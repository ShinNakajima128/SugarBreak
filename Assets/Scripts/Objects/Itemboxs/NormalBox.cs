using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBox : ItemboxBase
{
    public override void Damage(int attackDamage)
    {
        
        base.Damage(attackDamage);
        if (m_currentHp <= 0)
        {
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            mesh.enabled = false;

            var box = GetComponents<BoxCollider>();
            foreach (var col in box)
            {
                col.enabled = false;
            }
            EffectManager.PlayEffect(EffectType.EnemyDead, transform.position);
            if (m_playSeCount < 3)
            {
                SoundManager.Instance.PlaySeByName("Break");
                m_playSeCount++;
            }
            KonpeitouGenerator.Instance.GenerateKonpeitou(transform, m_konpeitouNum);
            StartCoroutine(Vanish(m_vanishTime));
        }
    }
}
