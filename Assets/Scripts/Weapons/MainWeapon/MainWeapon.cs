using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メイン武器のステータス
/// </summary>
public enum MainWeaponState
{
    /// <summary> 何も刺さってない </summary>
    None,
    /// <summary> 何か付属している </summary>
    Attach
}

public class MainWeapon : WeaponBase, IWeapon
{
    [SerializeField]
    float m_hitStopTime = 0.2f;

    [SerializeField]
    Transform m_hitEffectTrans = default;

    [SerializeField]
    Transform m_attachObjectParent = default;

    Coroutine coroutine;
    BoxCollider m_collider;
    Vector3 m_originColliderSize;
    int m_originAttackPower;
    MainWeaponState m_mainWeaponState = MainWeaponState.None;

    bool m_init = false;
    bool m_isJumpAttacked = false;

    void OnEnable()
    {
        if (!m_init)
        {
            m_collider = GetComponent<BoxCollider>();
            m_originColliderSize = m_collider.size;
            m_originAttackPower = AttackDamage;
            StartCoroutine(Setup());
            m_init = true;
        }
        else
        {
            m_collider.enabled = false;
            WeaponActionManager.ListenAction(ActionType.StartHitDecision, OnCollider);
            WeaponActionManager.ListenAction(ActionType.FinishHitDecision, OffCollider);
            WeaponActionManager.ListenAction(ActionType.WeaponEffect, OnEffect);
        }    
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
            Debug.Log($"現在の攻撃力：{attackDamage}");
            Debug.Log($"現在のColliderSize：{m_collider.size}");
            other.TryGetComponent<Rigidbody>(out var rb);
            switch (m_mainWeaponState)
            {
                case MainWeaponState.None:
                    target.Damage(attackDamage, rb, PlayerController.Instance.gameObject.transform.forward, 3);
                    break;
                case MainWeaponState.Attach:
                    target.Damage(attackDamage, rb, PlayerController.Instance.gameObject.transform.forward, 3 * 2);
                    var go = m_attachObjectParent.GetChild(0);
                    Destroy(go.gameObject);
                    m_mainWeaponState = MainWeaponState.None;
                    attackDamage = m_originAttackPower;
                    m_collider.size = m_originColliderSize;
                    EffectManager.PlayEffect(EffectType.Slam, m_attachObjectParent.position);
                    break;
            }

            if (coroutine == null)
            {
                coroutine = StartCoroutine(HitStop());
                EffectManager.PlayEffect(EffectType.Damage, m_hitEffectTrans.position);
            }
        }
        else
        {
            //武器の先に取り付けられるオブジェクトだったら
            if (other.gameObject.tag == "Stickable")
            {
                switch (m_mainWeaponState)
                {
                    case MainWeaponState.None:
                        other.transform.SetParent(m_attachObjectParent);
                        other.transform.localPosition = new Vector3(0 ,0, 0);
                        other.GetComponent<Collider>().enabled = false;
                        m_mainWeaponState = MainWeaponState.Attach;
                        SoundManager.Instance.PlaySeByName("Attach");
                        TryGetComponent<FieldSweets>(out var fs);
                        if (fs != null)
                        {
                            m_collider.enabled = true;
                            m_collider.enabled = false;
                            attackDamage = fs.AttackPower;
                            StartCoroutine(ColliderSizeChange(fs.ColliderSize));
                        }
                        break;
                    case MainWeaponState.Attach:
                        break;
                }       
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

    IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.1f);

        WeaponActionManager.ListenAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.ListenAction(ActionType.FinishHitDecision, OffCollider);
        WeaponActionManager.ListenAction(ActionType.WeaponEffect, OnEffect);
    }
    IEnumerator ColliderSizeChange(Vector3 afterSize)
    {
        m_collider.enabled = true;
        yield return null;
        m_collider.size = afterSize;
        yield return null;
        m_collider.enabled = false;
    }
}
