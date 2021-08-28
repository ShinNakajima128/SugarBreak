using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var targets = other.GetComponents<IDamagable>();
        
        if (targets != null)
        {
            foreach (var t in targets)
            {
                Debug.Log(t.ToString());
                t.Damage(m_damage);
            }
        }
        else
        {
            Debug.Log("当たらなかった");
        }
    }
}
