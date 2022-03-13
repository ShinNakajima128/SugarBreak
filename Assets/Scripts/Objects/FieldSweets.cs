using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージに存在するオブジェクトのデータを持つクラス
/// </summary>
public class FieldSweets : MonoBehaviour
{
    [SerializeField]
    FieldSweetsData m_sweetsData = default;

    public FieldSweetsSize SweetsType => m_sweetsData.SweetsSize;
    public Vector3 ColliderSize => m_sweetsData.ColliderSize;
    public int AttackPower => m_sweetsData.AttackPower;
    public int KonpeitouNum => m_sweetsData.KonpeitouNum;
    public int EnduranceCount => m_sweetsData.EnduranceCount;
}
