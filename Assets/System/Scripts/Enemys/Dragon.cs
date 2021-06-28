﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dragonstate
{
    Idle,
    Walk,
    Run,
    Dead,
    Attack
}

public class Dragon : EnemyBase
{
    public override void Damage(int attackPower)
    {
        if (m_damageEffect != null) Instantiate(m_damageEffect, this.transform.position, Quaternion.identity);

        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp > 0) m_anim.SetTrigger("Damage");

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            m_anim.SetBool("Dead", true);
            generator.GenerateKonpeitou(this.transform, enemyData.konpeitou);
            StartCoroutine(Vanish(EffectType.BossDead, m_vanishTime));
        }
    }
}