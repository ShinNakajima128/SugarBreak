using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeapon : WeaponBase, IWeapon
{
    [SerializeField]
    float m_hitStopTime = 0.2f;

    [SerializeField]
    Transform m_hitEffectTrans = default;

    Coroutine coroutine;
    BoxCollider m_collider;
    
    bool m_init = false;
    bool m_isJumpAttacked = false;

    void OnEnable()
    {
        if (!m_init)
        {
            m_collider = GetComponent<BoxCollider>();
            //m_effectPos = GameObject.FindGameObjectWithTag("CandyBeatEffectPosition").transform;
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

    public void WeaponAction1(Animator anim, Rigidbody rb)
    {
        attackDamage = 3;
        rb.velocity = Vector3.zero;
        anim.SetTrigger("MainAttack1");
        PlayerStatesManager.Instance.IsOperation = false;
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.5f));
    }

    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        //throw new System.NotImplementedException();
    }

    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        //throw new System.NotImplementedException();
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
    }

    /// <summary>
    /// ジャンプ強攻撃のエフェクトを再生する
    /// </summary>
    void OnEffect()
    {
        //EffectManager.PlayEffect(EffectType.Slam, m_effectPos.position);
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

    IEnumerator HitStop()
    {
        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(m_hitStopTime);

        Time.timeScale = 1.0f;

        coroutine = null;
    }
}
