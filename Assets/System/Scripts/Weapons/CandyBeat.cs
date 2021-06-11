﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBeat : WeaponBase
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Decolly>().Damage(attackDamage);
        }
    }
}
