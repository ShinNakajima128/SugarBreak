using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.System.Scripts.Enemys;


public class ExplosionController : WeaponBase
{
    int m_damage = 0;

    public int Damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    private void Start()
    {
        Destroy(this.gameObject, 0.8f);
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
