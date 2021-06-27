using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : EnemyBase
{
    [SerializeField] EnemyData dragonData = null;
    [SerializeField] float m_dragonVanishTime = 5.0f;
    bool isdead = false;
    public override void Damage(int attackPower)
    {
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0 && !isdead)
        {
            isdead = true;
            //m_anim.Play("Die");
            m_anim.SetBool("Dead", true);
            generator.GenerateKonpeitou(this.transform, dragonData.konpeitou);
            StartCoroutine(Vanish(m_dragonVanishTime));
        }
    }
}
