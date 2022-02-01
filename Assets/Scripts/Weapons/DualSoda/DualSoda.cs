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

    public void WeaponAction1(Animator anim, Rigidbody rb, int comboNum = 0)
    {
        throw new System.NotImplementedException();
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
