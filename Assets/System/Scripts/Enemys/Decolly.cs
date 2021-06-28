using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecollyState
{
    Idle,
    Move,
    Dead,
    Attack
}

public class Decolly : EnemyBase
{   
    public override void Damage(int attackPower)
    {
        //if (m_damageEffect != null) Instantiate(m_damageEffect, this.transform.position, Quaternion.identity);
        EffectManager.PlayEffect(EffectType.Damage, m_effectPos.position);

        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            m_anim.Play("Die");
            generator.GenerateKonpeitou(this.transform, enemyData.konpeitou);
            StartCoroutine(Vanish(EffectType.EnemyDead, m_vanishTime));
        }
    }
}
