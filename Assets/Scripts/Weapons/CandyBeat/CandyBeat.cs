using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBeat : WeaponBase
{
    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Damage(attackDamage);
        }
    }
}
