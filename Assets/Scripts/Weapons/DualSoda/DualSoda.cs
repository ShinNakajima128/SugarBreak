using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「デュアルソーダ」の機能を持つクラス
/// </summary>
public class DualSoda : WeaponBase, IWeapon
{
    [SerializeField] float m_hitStopTime = 0.02f;
    Coroutine coroutine;

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Damage(attackDamage);

            if (coroutine == null)
            {
                coroutine = StartCoroutine(HitStop());
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

    public void WeaponAction1(Animator anim, Rigidbody rb, Coroutine comboCor, int comboNum = 0)
    {
        if (comboNum == 3) return;

        if (comboNum == 0)
        {
            anim.SetTrigger("SwordAttack1");
            comboNum = 1;
            comboCor = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.3f));
        }
        else if (comboNum == 1)
        {
            anim.SetTrigger("SwordAttack2");
            comboNum = 2;
            if (comboCor != null)
            {
                StopCoroutine(comboCor);
                comboCor = null;
                comboCor = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.3f));
            }
        }
        else if (comboNum == 2)
        {
            rb.velocity = Vector3.zero;
            anim.SetTrigger("SwordAttack3");
            PlayerStatesManager.Instance.IsOperation = false;
            StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.0f));
            comboNum = 3;
            if (comboCor != null)
            {
                StopCoroutine(comboCor);
                comboCor = null;
                comboCor = StartCoroutine(PlayerController.Instance.AttackMotionTimer(0.5f));
            }
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
