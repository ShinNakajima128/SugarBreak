using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「キャンディビート」の機能を持つクラス
/// </summary>
public class CandyBeat : WeaponBase, IWeapon
{
    [SerializeField]
    float m_hitStopTime = 0.2f;

    [SerializeField]
    Transform m_hitEffectTrans = default;
    
    Coroutine coroutine;
    BoxCollider m_collider;
    Transform m_effectPos;
    bool m_init = false;
    bool m_isJumpAttacked = false;

    void OnEnable()
    {
        if (!m_init)
        {
            m_collider = GetComponent<BoxCollider>();
            m_effectPos = GameObject.FindGameObjectWithTag("CandyBeatEffectPosition").transform;
            m_init = true;
        }
        m_collider.enabled = false;
        WeaponActionManager.ListenAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.ListenAction(ActionType.FinishHitDecision, OffCollider);
        WeaponActionManager.ListenAction(ActionType.WeaponEffect, OnEffect);
    }

    void OnDisable()
    {
        WeaponActionManager.RemoveAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.RemoveAction(ActionType.FinishHitDecision, OffCollider);
        WeaponActionManager.RemoveAction(ActionType.WeaponEffect, OnEffect);
    }

   
    IEnumerator HitStop()
    {
        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(m_hitStopTime);

        Time.timeScale = 1.0f;

        coroutine = null;
    }

    /// <summary>
    /// 地上時の通常攻撃
    /// </summary>
    /// <param name="anim"> プレイヤーのAnimator </param>
    /// <param name="rb"> プレイヤーのRigidbody </param>
    public void WeaponAction1(Animator anim, Rigidbody rb)
    {
        attackDamage = 5;
        anim.SetBool("Light", true);
        PlayerStatesManager.Instance.IsOperation = false;
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.0f));
    }

    /// <summary>
    /// ジャンプ強攻撃
    /// </summary>
    /// <param name="anim"> プレイヤーのAnimator </param>
    /// <param name="rb"> プレイヤーのRigidbody </param>
    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        attackDamage = 10;
        m_isJumpAttacked = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        anim.SetBool("Strong", true);
        PlayerStatesManager.Instance.IsOperation = false;
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.0f));
    }

    /// <summary>
    /// 考え中
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="rb"></param>
    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 当たり判定を開始する
    /// </summary>
    void OnCollider()
    {
        m_collider.enabled = true;
        if (!m_isJumpAttacked)
        {
            SoundManager.Instance.PlaySeByName("LightAttack");
            SoundManager.Instance.PlayVoiceByName("univ0002");
        }
        else
        {
            SoundManager.Instance.PlaySeByName("JumpAttack");
            SoundManager.Instance.PlayVoiceByName("univ1257");
        }        
    }

    /// <summary>
    /// 当たり判定を終了する
    /// </summary>
    void OffCollider()
    {
        m_collider.enabled = false;
        m_isJumpAttacked = false;
    }

    /// <summary>
    /// ジャンプ強攻撃のエフェクトを再生する
    /// </summary>
    void OnEffect()
    {
        EffectManager.PlayEffect(EffectType.Slam, m_effectPos.position);
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Damage(attackDamage);

            if (coroutine == null)
            {
                coroutine = StartCoroutine(HitStop());
                EffectManager.PlayEffect(EffectType.Damage, m_hitEffectTrans.position);
            }
        }
    }
}
