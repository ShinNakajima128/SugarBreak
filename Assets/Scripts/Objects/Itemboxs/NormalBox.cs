using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBox : ItemboxBase
{
    public override void Damage(int attackPower, Rigidbody hitRb = null, Vector3 blowUpDir = default, float blowUpPower = 1)
    {
        
        base.Damage(attackPower);
        if (m_currentHp <= 0)
        {
            TryGetComponent<MeshRenderer>(out var mesh);
            if (mesh != null)
            {
                mesh.enabled = false;
            }
            else
            {
                var child =GetComponentInChildren<MeshRenderer>();
                if (child != null)
                {
                    child.enabled = false;
                }
            }

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
            ItemGenerator.Instance.GenerateKonpeitou(m_konpeitouNum, transform.position);
            StartCoroutine(Vanish(m_vanishTime));
        }
    }
}
