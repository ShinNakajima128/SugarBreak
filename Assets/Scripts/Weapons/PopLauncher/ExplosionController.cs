using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「ポップランチャー」の弾が爆発した時の機能を持つクラス
/// </summary>
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
                t.Damage(m_damage);
            }
        }
        else
        {
            Debug.Log("当たらなかった");
        }
    }
}
