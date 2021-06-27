﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decolly : EnemyBase
{
    [SerializeField] EnemyData decollyData = null;
    bool isdead = false;

    public override void Damage(int attackPower)
    {
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            m_anim.Play("Die");
            generator.GenerateKonpeitou(this.transform, decollyData.konpeitou);
            StartCoroutine(Vanish(m_vanishTime));
        }
    }

    void ShowData()
    {
        Debug.Log("最大HP：" + decollyData.maxHp + 
                  "攻撃力：" + decollyData.atk + 
                  "所持金平糖：" + decollyData.konpeitou);
    }
}
