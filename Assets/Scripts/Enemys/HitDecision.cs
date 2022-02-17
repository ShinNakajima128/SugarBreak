using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃判定を行うクラス
/// </summary>
public class HitDecision : MonoBehaviour
{
    /// <summary> 与えるダメージ </summary>
    public int AttackDamage { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }
        PlayerStatesManager.Instance.Damage(AttackDamage);
    }
}
