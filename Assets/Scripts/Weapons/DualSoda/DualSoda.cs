﻿using System.Collections;
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
        }
        m_collider.enabled = false;
        WeaponActionManager.ListenAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.ListenAction(ActionType.FinishHitDecision, OffCollider);
    }

    void OnDisable()
    {
        WeaponActionManager.RemoveAction(ActionType.StartHitDecision, OnCollider);
        WeaponActionManager.RemoveAction(ActionType.FinishHitDecision, OffCollider);
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Damage(attackDamage);

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
                SoundManager.Instance.PlaySeByName("DualSodaAttack");
                SoundManager.Instance.PlayVoiceByName("univ1254");
                break;
            case 2:
                SoundManager.Instance.PlaySeByName("DualSodaAttack");
                SoundManager.Instance.PlayVoiceByName("univ1255");
                break;
            case 3:
                m_collider.size = new Vector3(2f, 2f, 2f);
                SoundManager.Instance.PlaySeByName("DualSodaFinish");
                SoundManager.Instance.PlayVoiceByName("univ1256");
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

    public void WeaponAction1(Animator anim, Rigidbody rb)
    {
        if (comboNum == 3 || !PlayerStatesManager.Instance.IsOperation) return;

        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = null;
        }

        if (comboNum == 0)
        {
            Debug.Log("1回目");
            attackDamage = 2;
            anim.SetTrigger("SwordAttack1");
            comboNum = 1;
            comboCoroutine = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.4f, () =>
            {
                anim.SetBool("SwordAttack1", false);
                anim.SetBool("SwordAttack2", false);
                anim.SetBool("SwordAttack3", false);
                comboNum = 0;
            }));
        }
        else if (comboNum == 1)
        {
            Debug.Log("2回目");
            attackDamage = 4;
            anim.SetTrigger("SwordAttack2");
            comboNum = 2;
            comboCoroutine = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.4f, () =>
            {
                anim.SetBool("SwordAttack1", false);
                anim.SetBool("SwordAttack2", false);
                anim.SetBool("SwordAttack3", false);
                comboNum = 0;
            }));
        }
        else if (comboNum == 2)
        {
            Debug.Log("3回目");
            attackDamage = 8;
            rb.velocity = Vector3.zero;
            anim.SetTrigger("SwordAttack3");
            PlayerStatesManager.Instance.IsOperation = false;
            StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.0f));
            comboNum = 3;
            comboCoroutine = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.5f, () =>
            {
                anim.SetBool("SwordAttack1", false);
                anim.SetBool("SwordAttack2", false);
                anim.SetBool("SwordAttack3", false);
                comboNum = 0;
            }));
        }
    }

    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        throw new System.NotImplementedException();
    }

    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        throw new System.NotImplementedException();
    }
}
