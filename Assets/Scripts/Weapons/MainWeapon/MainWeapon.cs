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
    int m_currentSweetsKonpeitouNum = 0;
    int m_currentSweetsEnduranceCount = 0;
    bool m_init = false;

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
            WeaponActionManager.ListenAction(ActionType.Action2, OnDerivationAttack);
            WeaponActionManager.ListenAction(ActionType.SpecialAction, DisCard);
        }
    }
    void OnDisable()
    {
        WeaponActionManager.RemoveAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.RemoveAction(ActionType.FinishHitDecision, OffCollider);
        WeaponActionManager.RemoveAction(ActionType.WeaponEffect, OnEffect);
        WeaponActionManager.RemoveAction(ActionType.Action2, OnDerivationAttack);
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

    /// <summary>
    /// メイン武器の通常アクション
    /// </summary>
    /// <param name="anim"> PlayerのAnimator </param>
    /// <param name="rb"> PlayerのRigidbody </param>
    public void WeaponAction1(Animator anim, Rigidbody rb)
    {
        rb.velocity = Vector3.zero;
        
        //武器の状態によってアクションが変化
        switch (m_mainWeaponState)
        {
            case MainWeaponState.None:
                anim.SetTrigger("MainAttack1");
                StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.8f));
                break;
            case MainWeaponState.Attach:
                anim.SetTrigger("MainAttack2");
                StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.5f));
                break;
        }
    }

    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        //throw new System.NotImplementedException();

    }

    /// <summary>
    /// 装着しているお菓子を破棄するアクション
    /// </summary>
    /// <param name="anim"> PlayerのAnimator </param>
    /// <param name="rb"> PlayerのRigidbody </param>
    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        if (m_mainWeaponState == MainWeaponState.None)
        {
            return;
        }
        rb.velocity = Vector3.zero;
        attackDamage *= 2; 
        anim.SetTrigger("Discard");
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(2.0f));
    }

    /// <summary>
    /// 装備中のお菓子を破棄する
    /// </summary>
    void DisCard()
    {
        if (m_attachObjectParent.parent != null)
        {
            var go = m_attachObjectParent.GetChild(0);
            Destroy(go.gameObject);
            ItemGenerator.Instance.GenerateKonpeitou(m_currentSweetsKonpeitouNum, m_attachObjectParent.position);
            m_mainWeaponState = MainWeaponState.None;
            attackDamage = m_originAttackPower;
            m_collider.size = m_originColliderSize;
        }
        
        AudioManager.PlaySE(SEType.Weapon_Discard);
        AudioManager.PlayVOICE(VOICEType.Attack_Strike);
        EffectManager.PlayEffect(EffectType.Slam, m_attachObjectParent.position);
    }

    /// <summary>
    /// 当たり判定を開始する
    /// </summary>
    void OnCollider()
    {
        m_collider.enabled = true;
        AudioManager.PlaySE(SEType.Weapon_Thrust);

        switch (m_mainWeaponState)
        {
            case MainWeaponState.None:   
                AudioManager.PlayVOICE(VOICEType.Attack_Normal);
                break;
            case MainWeaponState.Attach:
                //AudioManager.PlayVOICE(VOICEType.Attack_Strike);
                break;
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

    /// <summary>
    /// メイン武器の派生技のイベント
    /// </summary>
    void OnDerivationAttack()
    {
        AudioManager.PlayVOICE(VOICEType.Attack_Finish);
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
                    VibrationController.OnVibration(Strength.Low, 0.3f);
                    break;
                case MainWeaponState.Attach:
                    target.Damage(attackDamage, rb, PlayerController.Instance.gameObject.transform.forward, attackDamage);
                    VibrationController.OnVibration(Strength.Middle, 0.3f);
                    m_currentSweetsEnduranceCount--;
                    if (m_currentSweetsEnduranceCount <= 0)
                    {
                        var go = m_attachObjectParent.GetChild(0);
                        Destroy(go.gameObject);
                        m_mainWeaponState = MainWeaponState.None;
                        attackDamage = m_originAttackPower;
                        m_collider.size = m_originColliderSize;
                        EffectManager.PlayEffect(EffectType.Slam, m_attachObjectParent.position);
                    }
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
                        other.transform.SetParent(m_attachObjectParent);    //お菓子をメイン武器の子オブジェクトにする
                        other.transform.localPosition = new Vector3(0, 0, 0);
                        other.GetComponent<Collider>().enabled = false;
                        m_mainWeaponState = MainWeaponState.Attach;     //メイン武器のステータスを強化状態に変更
                        AudioManager.PlaySE(SEType.Weapon_Attach);
                        VibrationController.OnVibration(Strength.Low,  0.2f);

                        other.TryGetComponent<FieldSweets>(out var fs); //取り付けたお菓子のデータを取得
                        if (fs != null)
                        {
                            m_collider.enabled = true;
                            m_collider.enabled = false;
                            attackDamage = fs.AttackPower;
                            m_collider.size = fs.ColliderSize;
                            m_currentSweetsKonpeitouNum = fs.KonpeitouNum;
                            m_currentSweetsEnduranceCount = fs.EnduranceCount;
                            Debug.Log($"{other.gameObject.name}の金平糖数：{m_currentSweetsKonpeitouNum}");
                            Debug.Log($"{other.gameObject.name}の耐久力：{m_currentSweetsEnduranceCount}");
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
        WeaponActionManager.ListenAction(ActionType.Action2, OnDerivationAttack);
        WeaponActionManager.ListenAction(ActionType.SpecialAction, DisCard);
    }
}
