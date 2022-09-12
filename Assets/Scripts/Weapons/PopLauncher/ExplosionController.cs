using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「ポップランチャー」の弾が爆発した時の機能を持つクラス
/// </summary>
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
        var targets = other.GetComponents<IDamagable>();
        
        if (targets != null)
        {
            foreach (var t in targets)
            {
                t.Damage(m_damage);
                EffectManager.PlayEffect(EffectType.Damage, t.EffectTarget.position);
                Debug.Log(t.ToString());
            }
        }
        else
        {
            Debug.Log("当たらなかった");
        }
    }
}
