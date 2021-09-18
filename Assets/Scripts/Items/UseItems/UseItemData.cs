using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create UseItemData")]
public class UseItemData : ScriptableObject
{
    [SerializeField]
    string m_itemName = default;

    [SerializeField]
    int m_healValue = default;

    [SerializeField]
    int m_cost = default;

    [SerializeField]
    Sprite m_itemIcon = default;

    public string ItemName => m_itemName;

    public int HealValue => m_healValue;

    public int Cost => m_cost;

    public Sprite ItemIcon => m_itemIcon;
}
