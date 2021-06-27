using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decolly : EnemyBase
{
    public enum DecollyState
    {
        Idle,
        Move,
        Dead,
        Attack
    }

    public override void Damage(int attackPower)
    {
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            m_anim.Play("Die");
            generator.GenerateKonpeitou(this.transform, enemyData.konpeitou);
            StartCoroutine(Vanish(m_vanishTime));
        }
    }
}
