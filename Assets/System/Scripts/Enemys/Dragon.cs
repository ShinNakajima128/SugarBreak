using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : EnemyBase
{
    public enum Dragonstate
    {
        Idle,
        Walk,
        Run,
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
            m_anim.SetBool("Dead", true);
            generator.GenerateKonpeitou(this.transform, enemyData.konpeitou);
            StartCoroutine(Vanish(m_vanishTime));
        }
    }
}
