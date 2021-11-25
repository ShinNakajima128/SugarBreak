using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消費アイテムの基底クラス
/// </summary>
public class UseItemBase : ScriptableObject
{
    [SerializeField]
    string m_itemName = default;

    [SerializeField]
    int m_healValue = default;

    [SerializeField]
    int m_cost = default;

    [SerializeField]
    Sprite m_itemIcon = default;

    public int HealValue => m_healValue;

    public int Cost => m_cost;

    public Sprite ItemIcon => m_itemIcon;

    public virtual void Use(int hp)
    {
        hp += m_healValue;
    }
}
