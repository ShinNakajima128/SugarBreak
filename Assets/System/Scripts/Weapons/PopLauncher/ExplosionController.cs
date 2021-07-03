using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.System.Scripts.Damage;


public class ExplosionController : WeaponBase
{
    int m_damage = 10;

    public int Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null && other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(target);
            target.Damage(m_damage);
        }
    }
}
