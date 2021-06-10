using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decolly : EnemyBase
{
    Animator m_anim;

    private void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    public override void Damage(int attackPower)
    {
        base.Damage(attackPower);

        if (currentHp <= 0)
        {
            m_anim.Play("Die");
            Destroy(this.gameObject, 2.0f);
        }
    }
}
