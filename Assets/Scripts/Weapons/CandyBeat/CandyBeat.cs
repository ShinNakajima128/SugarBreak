using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「キャンディビート」の機能を持つクラス
/// </summary>
public class CandyBeat : WeaponBase
{
    [SerializeField] float m_hitStopTime = 0.2f;
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
}
