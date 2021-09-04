using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum DecollyState
{
    Idle,
    Move,
    Chase,
    Attack,
    Freeze,
    Dead
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(SetPosition))]
public class Decolly : EnemyBase
{
    [Header("動く速さ")]
    [SerializeField] 
    float m_moveSpeed = 1.0f;

    [Header("索敵範囲")]
    [SerializeField] 
    SphereCollider searchArea;

    [Header("索敵の角度")]
    [SerializeField] 
    float searchAngle = 130f;

    [Header("待ち時間")]
    [SerializeField]
    private float waitTime = 5f;

    /// <summary> 速度 </summary>
    Vector3 velocity;
    /// <summary> 移動方向 </summary>
    Vector3 direction;
    /// <summary> 到着フラグ </summary>
    private bool arrived;
    /// <summary> 敵のステータス </summary>
    DecollyState decollyState;
    SetPosition setPosition;
    
    //　経過時間
    private float elapsedTime;
    //　プレイヤーTransform
    private Transform playerTransform;
    CharacterController characterController;
    public event Action AttackStartAction;
    public event Action AttackEndAction;
    public event Action StateEndAction;

    private void Start()
    {
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition();
        characterController = GetComponent<CharacterController>();
        decollyState = DecollyState.Idle;
    }

    private void Update()
    {
        if (decollyState == DecollyState.Move || decollyState == DecollyState.Chase)
        {
            if (decollyState == DecollyState.Chase)
            {
                setPosition.SetDestination(playerTransform.position);
            }

            if (characterController.isGrounded)
            {
                velocity = Vector3.zero;
                m_anim.SetFloat("Speed", 2.0f);
                direction = (setPosition.GetDestination() - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                velocity = direction * m_moveSpeed;
            }

            if (decollyState == DecollyState.Move)
            {
                //　目的地に到着したかどうかの判定
                if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.7f)
                {
                    SetState(DecollyState.Idle);
                    m_anim.SetFloat("Speed", 0.0f);
                }
            }
            else if (decollyState == DecollyState.Chase)
            {
                //　攻撃する距離だったら攻撃
                if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 1f)
                {
                    SetState(DecollyState.Attack);
                }
            }
        }
        else if (decollyState == DecollyState.Freeze)
        {
            elapsedTime += Time.deltaTime;
            transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(DecollyState.Idle);
            }
        }

        //　目的地に到着したかどうかの判定
        if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.5f)
        {
            SetState(DecollyState.Idle);
            m_anim.SetFloat("Speed", 0.0f);
        }
        else if (decollyState == DecollyState.Idle)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(DecollyState.Move);
            }
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        if (characterController.enabled) characterController.Move(velocity * Time.deltaTime);
    }

    public void SetState(DecollyState tempState, Transform target = null)
    {
        if (decollyState == DecollyState.Dead) return;

        decollyState = tempState;

        if (tempState == DecollyState.Dead)
        {
            Dead();
        }
        else if (decollyState == DecollyState.Move)
        {
            arrived = false;
            elapsedTime = 0f;
            decollyState = tempState;
            setPosition.CreateRandomPosition();
        }
        else if (tempState == DecollyState.Chase)
        {
            decollyState = tempState;
            //　待機状態から追いかける場合もあるのでOff
            arrived = false;
            //　追いかける対象をセット
            playerTransform = target;
        }
        else if (tempState == DecollyState.Idle)
        {
            elapsedTime = 0f;
            decollyState = tempState;
            arrived = true;
            velocity = Vector3.zero;
            m_anim.SetFloat("Speed", 0f);
        }
        else if (tempState == DecollyState.Attack)
        {
            velocity = Vector3.zero;
            m_anim.SetFloat("Speed", 0f);
            m_anim.SetBool("Attack", true);
        }
        else if (tempState == DecollyState.Freeze)
        {
            elapsedTime = 0f;
            velocity = Vector3.zero;
            m_anim.SetFloat("Speed", 0f);
            m_anim.SetBool("Attack", false);
        }     
    }

    public void AttackStart()
    {
        AttackStartAction?.Invoke();
    }

    public void AttackEnd()
    {
        AttackEndAction?.Invoke();
    }

    public void StateEnd()
    {
        StateEndAction?.Invoke();
    }
    void Dead()
    {
        arrived = true;
        m_anim.SetBool("Dead", true);
        m_anim.Play("Die");
        characterController.enabled = false;
        KonpeitouGenerator.Instance.GenerateKonpeitou(this.transform, enemyData.konpeitou, 2);
        StartCoroutine(Vanish(EffectType.EnemyDead, m_vanishTime));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isdead)
        {
            //　主人公の方向
            var playerDirection = other.transform.position - transform.position;
            //　敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったら発見
            if (angle <= searchAngle)
            {
                //Debug.Log("主人公発見: " + angle);
                if (decollyState != DecollyState.Chase && decollyState != DecollyState.Freeze)
                {
                    SetState(DecollyState.Chase, other.transform);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isdead)
        {
            SetState(DecollyState.Idle);
        }
    }

    public override void Damage(int attackPower)
    {
        EffectManager.PlayEffect(EffectType.Damage, m_effectPos.position);

        currentHp -= attackPower;
        m_HpSlider.value = currentHp;
        SoundManager.Instance.PlaySeByName("Damage");

        if (currentHp > 0) m_anim.SetTrigger("Damage");

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            SetState(DecollyState.Dead);
        }
    }

#if UNITY_EDITOR
    //　サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
    }
#endif
}
