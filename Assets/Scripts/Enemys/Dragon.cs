using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum DragonState
{
    Idle,
    Chase,
    Freeze,
    Dead,
    Attack
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(SetPosition))]
public class Dragon : EnemyBase
{
    [SerializeField] 
    float m_moveSpeed = 1.0f;

    [SerializeField]
    float waitTime = 2.0f;

    [SerializeField] 
    SphereCollider searchArea;

    [SerializeField] 
    float m_distanceToPlayer = 1.5f;

    /// <summary> 索敵の角度 </summary>
    [SerializeField] 
    float searchAngle = 130f;
    /// <summary> 到着フラグ </summary>
    bool arrived;
    /// <summary> 速度 </summary>
    Vector3 velocity;
    /// <summary> 移動方向 </summary>
    Vector3 direction;
    Transform playerTransform;
    SetPosition setPosition;
    float elapsedTime;
    CharacterController characterController;
    DragonState dragonState;
    public event Action AttackStartAction;
    public event Action AttackEndAction;
    public event Action StateEndAction;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        setPosition = GetComponent<SetPosition>();
        dragonState = DragonState.Idle;
    }

    private void Update()
    {
        if (!BossArea.isBattle) return;

        if (dragonState == DragonState.Freeze)
        {
            elapsedTime += Time.deltaTime;
            transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(DragonState.Idle);
            }
        }
        else if (dragonState == DragonState.Chase)
        {
            if (dragonState == DragonState.Chase)
            {
                setPosition.SetDestination(playerTransform.position);

                //　攻撃する距離だったら攻撃
                if (Vector3.Distance(transform.position, setPosition.GetDestination()) < m_distanceToPlayer)
                {
                    SetState(DragonState.Attack);
                }
            }

            if (characterController.isGrounded)
            {
                velocity = Vector3.zero;
                m_anim.SetFloat("Speed", 2.0f);
                direction = (setPosition.GetDestination() - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                velocity = direction * m_moveSpeed;
            }
        }
        

        //　目的地に到着したかどうかの判定
        if (Vector3.Distance(transform.position, setPosition.GetDestination()) < m_distanceToPlayer)
        {
            SetState(DragonState.Idle);
            m_anim.SetFloat("Speed", 0.0f);
        }
        else if (dragonState == DragonState.Idle)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(DragonState.Chase);
            }
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        if (characterController.enabled) characterController.Move(velocity * Time.deltaTime);
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

    public override void Damage(int attackPower)
    {
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp > 0) m_anim.SetTrigger("Damage");

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            SetState(DragonState.Dead);
        }
    }

    void Dead()
    {
        arrived = true;
        m_anim.SetBool("Dead", true);
        characterController.enabled = false;
        //KonpeitouGenerator.Instance.GenerateKonpeitou(this.transform, enemyData.konpeitou);
        StartCoroutine(Vanish(EffectType.BossDead, m_vanishTime));
    }

    public void SetState(DragonState tempState, Transform target = null)
    {
        if (dragonState == DragonState.Dead) return;

        dragonState = tempState;

        if (tempState == DragonState.Dead)
        {
            Dead();
        }
        else if (tempState == DragonState.Chase)
        {
            dragonState = tempState;
            //　待機状態から追いかける場合もあるのでOff
            arrived = false;
            //　追いかける対象をセット
            playerTransform = target;
        }
        else if (tempState == DragonState.Idle)
        {
            elapsedTime = 0f;
            dragonState = tempState;
            arrived = true;
            velocity = Vector3.zero;
            m_anim.SetFloat("Speed", 0f);
        }
        else if (tempState == DragonState.Attack)
        {
            velocity = Vector3.zero;
            m_anim.SetFloat("Speed", 0f);
            m_anim.SetBool("Attack_1", true);
        }
        else if (tempState == DragonState.Freeze)
        {
            elapsedTime = 0f;
            velocity = Vector3.zero;
            m_anim.SetFloat("Speed", 0f);
            m_anim.SetBool("Attack_1", false);
        }
    }

    public void GrowlSe()
    {
        SoundManager.Instance.PlaySeByName("Growl");
    }
    public void SwingSe()
    {
        SoundManager.Instance.PlaySeByName("Swing");
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
                if (dragonState != DragonState.Chase && dragonState != DragonState.Freeze)
                {
                    SetState(DragonState.Chase, other.transform);
                }
            }
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
