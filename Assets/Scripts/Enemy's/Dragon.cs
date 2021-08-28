using System.Collections;
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
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp > 0) m_anim.SetTrigger("Damage");

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            m_anim.SetBool("Dead", true);
            KonpeitouGenerator.Instance.GenerateKonpeitou(this.transform, enemyData.konpeitou, 2);
            StartCoroutine(Vanish(EffectType.BossDead, m_vanishTime));
        }
    }
}
