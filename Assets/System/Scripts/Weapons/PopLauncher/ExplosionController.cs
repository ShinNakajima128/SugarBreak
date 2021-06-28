﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.System.Scripts.Enemys;


public class ExplosionController : WeaponBase
{
    int m_damage = 5;

    public int Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            //Debug.Log(m_damage);
            target.Damage(m_damage);
        }
    }
}
