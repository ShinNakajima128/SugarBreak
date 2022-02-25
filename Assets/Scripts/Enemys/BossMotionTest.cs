using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのステータス
/// </summary>
public enum EnemyState
{
    Idle,
    Move,
    Attack,
    dead
}

/// <summary>
/// ボスの機能のテスト用クラス
/// </summary>
public class BossMotionTest : MonoBehaviour
{
    /// <summary> 攻撃力 </summary>
    [SerializeField]
    int m_attackPower = 2;

    /// <summary> 移動速度 </summary>
    [SerializeField]
    float m_moveSpeed = 5.0f;

    /// <summary> 回転速度 </summary>
    [SerializeField]
    float m_turnSpeed = 3.0f;

    [SerializeField]
    float m_distanceToPlayer = 0.1f;

    /// <summary> 歩行時のエフェクトを出す位置 </summary>
    [SerializeField]
    Transform m_walkEffectPos = default;

    /// <summary> 攻撃のエフェクトを出す位置 </summary>
    [SerializeField]
    Transform m_attackEffectPos = default;

    /// <summary> 攻撃の当たり判定用のCollider </summary>
    [SerializeField]
    BoxCollider m_attackCollider = default;

    /// <summary> Playerにヒットしたか判定するクラス </summary>
    [SerializeField]
    HitDecision m_hd = default;

    /// <summary> 敵のステータス </summary>
    EnemyState m_states = EnemyState.Idle;
    /// <summary> 敵を動かすためのコンポーネント </summary>
    CharacterController m_cc = default;
    /// <summary> アニメーションのコンポーネント </summary>
    Animator m_anim = default;
    /// <summary> 子オブジェクトにある索敵用のコンポーネント </summary>
    PlayerSearcher m_ps = default;
    /// <summary> 移動方向 </summary>
    Vector3 m_direction = default;
    /// <summary> 速度 </summary>
    Vector3 m_velocity = default;
    public bool IsWaited { get; private set; }
    /// <summary> 次の状態に遷移するまでの時間 </summary>
    public float WaitStatesTime { get; set; } = 3.0f;

    /// <summary> 現在の敵のステータス </summary>
    public EnemyState CurrentState { get => m_states; set => m_states = value; }

    void Start()
    {
        m_cc = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
        m_ps = GetComponentInChildren<PlayerSearcher>();
        StartCoroutine(ChangeState(EnemyState.Idle));
    }

    private void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// 毎フレーム
    /// </summary>
    void UpdateState()
    {
        if (!IsWaited)
        {
            switch (m_states)
            {
                case EnemyState.Idle:
                    if (m_ps.IsWithinRange)
                    {
                        StartCoroutine(ChangeState(EnemyState.Move));
                    }
                    break;
                case EnemyState.Move:
                    MoveAction();
                    break;
                case EnemyState.Attack:
                    StartCoroutine(ChangeState(EnemyState.Idle));
                    break;
                case EnemyState.dead:
                    break;
            }
        }
    }

    void MoveAction()
    {
        m_direction = (m_ps.PlayerPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(m_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);
        //transform.LookAt(new Vector3(m_ps.PlayerPosition.x, transform.position.y, m_ps.PlayerPosition.z));
        m_velocity = m_direction * m_moveSpeed;
        m_velocity.y += Physics.gravity.y * Time.deltaTime;

        if (m_cc.enabled)
        {
            m_cc.Move(m_velocity * Time.deltaTime);
            Debug.Log(m_ps.PlayerPosition);
        };

        //Debug.Log($"追跡中:距離{Vector3.Distance(transform.position, m_ps.PlayerTrans.position)}");
        //　攻撃する距離だったら攻撃
        if (Vector3.Distance(transform.position, m_ps.PlayerPosition) < m_distanceToPlayer && m_ps.IsFind)
        {
            StartCoroutine(ChangeState(EnemyState.Attack, 3.4f));
            Debug.Log("攻撃");
        }
    }

    /// <summary>
    /// ステータスを変更する
    /// </summary>
    /// <param name="state"> 変更するステータス </param>
    IEnumerator ChangeState(EnemyState state, float waitTime = 0.02f)
    {
        //以前のステータスを保持
        var prev = m_states;

        m_states = state;

        Debug.Log($"{prev}から{state}へ遷移");

        IsWaited = true;

        //ステータスが変更された時に1度だけ処理を行う
        switch (m_states)
        {
            case EnemyState.Idle:
                m_anim.Play("Idle");
                break;
            case EnemyState.Move:
                //m_anim.Play("Move");
                m_anim.CrossFadeInFixedTime("Move", 0.1f);
                break;
            case EnemyState.Attack:
                //m_anim.Play("Attack");
                m_anim.CrossFadeInFixedTime("Attack", 0.1f);
                break;
            case EnemyState.dead:
                m_anim.Play("Dead");
                break;
        }

        yield return new WaitForSeconds(waitTime);
        IsWaited = false;
    }

    /// <summary>
    /// 歩く
    /// </summary>
    public void Walk()
    {
        EventManager.OnEvent(Events.CameraShake); //カメラを揺らす
        SoundManager.Instance.PlaySeByName("怪獣の足音");
        //EffectManager.PlayEffect(EffectType.Landing, m_walkEffectPos.position);
    }

    /// <summary>
    /// 攻撃1
    /// </summary>
    public void Attack1()
    {
        EventManager.OnEvent(Events.CameraShake); //カメラを揺らす
        SoundManager.Instance.PlaySeByName("全力で踏み込む");
        EffectManager.PlayEffect(EffectType.Landing, m_attackEffectPos.position);
        m_hd.AttackDamage = m_attackPower;
    }

    /// <summary>
    /// 攻撃1の当たり判定をONにする
    /// </summary>
    public void OnAttack1Collider()
    {
        m_attackCollider.enabled = true;
    }
    /// <summary>
    /// 攻撃1の当たり判定をOFFにする
    /// </summary>
    public void OffAttack1Collider()
    {
        m_attackCollider.enabled = false;
    }
}
