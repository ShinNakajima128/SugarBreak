using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃判定を行うクラス
/// </summary>
public class HitDecision : MonoBehaviour
{
    public int AttackDamage { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Damage(AttackDamage);
        }
    }
}
