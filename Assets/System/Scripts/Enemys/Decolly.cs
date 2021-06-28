using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum DecollyState
{
    Idle,
    Move,
    Chase,
    Attack,
    Dead
}

public class Decolly : EnemyBase
{
    /// <summary> 動く速さ </summary>
    [SerializeField] float m_moveSpeed = 1.0f;
    /// <summary> 速度 </summary>
    Vector3 velocity;
    /// <summary> 移動方向 </summary>
    Vector3 direction;
    /// <summary> 到着フラグ </summary>
    private bool arrived;
    /// <summary> 索敵範囲 </summary>
    [SerializeField] SphereCollider searchArea;
    /// <summary> 索敵の角度 </summary>
    [SerializeField] float searchAngle = 130f;
    /// <summary> 敵のステータス </summary>
    DecollyState decollyState;
    SetPosition setPosition;
    //　待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //　経過時間
    private float elapsedTime;
    //　プレイヤーTransform
    private Transform playerTransform;
    CharacterController characterController;

    private void Start()
    {
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition();
        characterController = GetComponent<CharacterController>();
        decollyState = DecollyState.Idle;
    }

    private void Update()
    {
        //if (decollyState == DecollyState.Move || decollyState == DecollyState.Chase)
        //{
        //    if (decollyState == DecollyState.Chase)
        //    {
        //        setPosition.SetDestination(playerTransform.position);
        //    }

        //    if (characterController.isGrounded)
        //    {
        //        velocity = Vector3.zero;
        //        m_anim.SetFloat("Speed", 2.0f);
        //        direction = (setPosition.GetDestination() - transform.position).normalized;
        //        transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
        //        velocity = direction * m_moveSpeed;
        //    }   
        //}
        ////　目的地に到着したかどうかの判定
        //if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.5f)
        //{
        //    SetState(DecollyState.Idle);
        //    m_anim.SetFloat("Speed", 0.0f);
        //}
        //else if (decollyState == DecollyState.Idle)
        //{
        //    elapsedTime += Time.deltaTime;

        //    //　待ち時間を越えたら次の目的地を設定
        //    if (elapsedTime > waitTime)
        //    {
        //        SetState(DecollyState.Move);
        //    }
        //}
        //velocity.y += Physics.gravity.y * Time.deltaTime;
        //characterController.Move(velocity * Time.deltaTime);
    }

    public void SetState(DecollyState tempState, Transform target = null)
    {
        if (decollyState == DecollyState.Move)
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
    }

    void Dead()
    {
        m_anim.Play("Die");
        generator.GenerateKonpeitou(this.transform, enemyData.konpeitou);
        StartCoroutine(Vanish(EffectType.EnemyDead, m_vanishTime));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //　主人公の方向
            var playerDirection = other.transform.position - transform.position;
            //　敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったら発見
            if (angle <= searchAngle)
            {
                Debug.Log("主人公発見: " + angle);
                if (decollyState != DecollyState.Chase)
                {
                    SetState(DecollyState.Chase, other.transform);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("見失った");
            SetState(DecollyState.Idle);
        }
    }

    public override void Damage(int attackPower)
    {
        //if (m_damageEffect != null) Instantiate(m_damageEffect, this.transform.position, Quaternion.identity);
        EffectManager.PlayEffect(EffectType.Damage, m_effectPos.position);

        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            //decollyState = DecollyState.Dead;
            Dead();
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
