﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBox : ItemboxBase
{
    [SerializeField] float m_popPower = 5;
    public override void Damage(int attackDamage)
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
        base.Damage(attackDamage);
        if (m_currentHp <= 0)
        {
            var box = GetComponents<BoxCollider>();
            foreach (var col in box)
            {
                col.enabled = false;
            }
            EffectManager.PlayEffect(EffectType.EnemyDead, transform.position);
            SoundManager.Instance.PlaySeByName("Break");
            KonpeitouGenerator.Instance.GenerateKonpeitou(transform, m_konpeitouNum, m_popPower);
            StartCoroutine(Vanish(m_vanishTime));
        }
        
        Debug.Log("NormalBoxに当たった");
    }
}