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

    [SerializeField]
    bool m_debugMode = false;

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
            WeaponActionManager.ListenAction(ActionType.SpecialAction, DisCard);
        }
    }
    void OnDisable()
    {
        WeaponActionManager.RemoveAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.RemoveAction(ActionType.FinishHitDecision, OffCollider);
        WeaponActionManager.RemoveAction(ActionType.WeaponEffect, OnEffect);
        WeaponActionManager.RemoveAction(ActionType.SpecialAction, DisCard);
    }

    void Update()
    {
        if (m_debugMode)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                m_collider.size = Vector3.zero;
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                m_collider.size = m_originColliderSize;
            }
        }
    }

    public void WeaponAction1(Animator anim, Rigidbody rb)
    {
        rb.velocity = Vector3.zero;
        anim.SetTrigger("MainAttack1");
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.5f));
    }

    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        //throw new System.NotImplementedException();
    }

    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        if (m_mainWeaponState == MainWeaponState.None)
        {
            Debug.Log("miss");
            return;
        }
        Debug.Log("call");
        rb.velocity = Vector3.zero;
        anim.SetTrigger("Discard");
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(2.0f));
    }

    /// <summary>
    /// 装備中のお菓子を破棄する
    /// </summary>
    void DisCard()
    {
        var go = m_attachObjectParent.GetChild(0);
        Destroy(go.gameObject);
        ItemGenerator.Instance.GenerateKonpeitou(10, m_attachObjectParent.position);
        m_mainWeaponState = MainWeaponState.None;
        attackDamage = m_originAttackPower;
        m_collider.size = m_originColliderSize;
        SoundManager.Instance.PlaySeByName("Damage2");
        SoundManager.Instance.PlayVoiceByName("univ1257");
        EffectManager.PlayEffect(EffectType.Slam, m_attachObjectParent.position);
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
    /// 攻撃のエフェクトを再生する
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
            other.TryGetComponent<Rigidbody>(out var rb);
            switch (m_mainWeaponState)
            {
                case MainWeaponState.None:
                    target.Damage(attackDamage, rb, PlayerController.Instance.gameObject.transform.forward, attackDamage);
                    break;
                case MainWeaponState.Attach:
                    target.Damage(attackDamage, rb, PlayerController.Instance.gameObject.transform.forward, attackDamage);
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
                        other.transform.localPosition = new Vector3(0, 0, 0);
                        other.GetComponent<Collider>().enabled = false;
                        m_mainWeaponState = MainWeaponState.Attach;
                        SoundManager.Instance.PlaySeByName("Attach");
                        other.TryGetComponent<FieldSweets>(out var fs);
                        if (fs != null)
                        {
                            m_collider.enabled = true;
                            m_collider.enabled = false;
                            StartCoroutine(ColliderSizeChange(fs.AttackPower, fs.ColliderSize));
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
        WeaponActionManager.ListenAction(ActionType.SpecialAction, DisCard);
    }
    IEnumerator ColliderSizeChange(int afterPower, Vector3 afterSize)
    {
        m_collider.enabled = true;
        yield return null;
        attackDamage = afterPower;
        m_collider.size = afterSize;
        Debug.Log($"現在の攻撃力：{attackDamage}");
        Debug.Log($"現在のColliderSize：{m_collider.size}");
        yield return null;
        m_collider.enabled = false;
    }
}
