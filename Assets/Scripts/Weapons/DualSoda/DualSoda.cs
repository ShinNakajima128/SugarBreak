using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「デュアルソーダ」の機能を持つクラス
/// </summary>
public class DualSoda : WeaponBase, IWeapon
{
    [SerializeField] float m_hitStopTime = 0.02f;
    Coroutine coroutine = default;
    Coroutine comboCoroutine = default;
    BoxCollider m_collider;
    bool m_init = false;
    bool onCombo = false;
    int comboNum = 0;

    void OnEnable()
    {
        if (!m_init)
        {
            m_collider = GetComponent<BoxCollider>();
            m_init = true;
            m_collider.enabled = false;
            WeaponActionManager.ListenAction(ActionType.StartHitDecision, OnCollider);
            WeaponActionManager.ListenAction(ActionType.FinishHitDecision, OffCollider);
        }
        else
        {
            WeaponActionManager.ListenAction(ActionType.StartHitDecision, OnCollider);
            WeaponActionManager.ListenAction(ActionType.FinishHitDecision, OffCollider);
        }   
    }

    void OnDisable()
    {
        WeaponActionManager.RemoveAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.RemoveAction(ActionType.FinishHitDecision, OffCollider);
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        other.TryGetComponent<Rigidbody>(out var rb);
        if (target != null)
        {
            target.Damage(attackDamage, rb, PlayerController.Instance.gameObject.transform.forward, 3);
            EffectManager.PlayEffect(EffectType.Damage, other.gameObject.transform.position);

            if (coroutine == null)
            {
                Debug.Log("callback");
                coroutine = StartCoroutine(HitStop());
            }
        }
    }

    /// <summary>
    /// 当たり判定を開始する
    /// </summary>
    void OnCollider()
    {
        Debug.Log(comboNum);
        m_collider.enabled = true;
        switch (comboNum)
        {
            case 1:
                AudioManager.PlaySE(SEType.Weapon_Combo);
                AudioManager.PlayVOICE(VOICEType.Attack_Combo_First);
                break;
            case 2:
                AudioManager.PlaySE(SEType.Weapon_Combo);
                AudioManager.PlayVOICE(VOICEType.Attack_Combo_Second);
                break;
            case 3:
                m_collider.size = new Vector3(2f, 2f, 2f);
                AudioManager.PlaySE(SEType.Weapon_Finish);
                AudioManager.PlayVOICE(VOICEType.Attack_Finish);
        break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// 当たり判定を終了する
    /// </summary>
    void OffCollider()
    {
        m_collider.enabled = false;

        m_collider.size = new Vector3(1.2f, 1.2f, 1.2f);
    }

    IEnumerator HitStop()
    {
        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(m_hitStopTime);

        Time.timeScale = 1.0f;

        coroutine = null;
    }

    /// <summary>
    /// 地上時の連続コンボ攻撃
    /// </summary>
    /// <param name="anim"> プレイヤーのAnimator </param>
    /// <param name="rb"> プレイヤーのRigidbody </param>
    public void WeaponAction1(Animator anim, Rigidbody rb)
    {
        if (comboNum == 3 || !PlayerStatesManager.Instance.IsOperation)
        {
            Debug.Log("入力受付外");
            return;
        }

        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = null;
        }

        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        if (comboNum == 0)
        {
            attackDamage = 2;
            anim.SetTrigger("SwordAttack1");
            comboNum = 1;
            comboCoroutine = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.4f, () =>
            {
                anim.SetBool("SwordAttack1", false);
                anim.SetBool("SwordAttack2", false);
                anim.SetBool("SwordAttack3", false);
                comboNum = 0;
                Debug.Log("コンボリセット");
            }));
        }
        else if (comboNum == 1)
        {
            attackDamage = 4;
            anim.SetTrigger("SwordAttack2");
            comboNum = 2;
            comboCoroutine = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.4f, () =>
            {
                anim.SetBool("SwordAttack1", false);
                anim.SetBool("SwordAttack2", false);
                anim.SetBool("SwordAttack3", false);
                comboNum = 0;
                Debug.Log("コンボリセット");
            }));
        }
        else if (comboNum == 2)
        {
            attackDamage = 8;
            rb.velocity = Vector3.zero;
            anim.SetTrigger("SwordAttack3");
            PlayerStatesManager.Instance.IsOperation = false;
            StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.0f));
            comboNum = 3;
            comboCoroutine = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.2f, () =>
            {
                anim.SetBool("SwordAttack1", false);
                anim.SetBool("SwordAttack2", false);
                anim.SetBool("SwordAttack3", false);
                comboNum = 0;
                Debug.Log("コンボリセット");
            }));
        }
    }

    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        //throw new System.NotImplementedException();
    }

    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        //throw new System.NotImplementedException();
    }
}
