﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("ヒットストップ呼び出し");

        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(m_hitStopTime);

        Time.timeScale = 1.0f;

        coroutine = null;
    }
}
