using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create ItemboxData")]
public class ItemboxData : ScriptableObject
{
    [SerializeField] int m_maxHp = 10;
    [SerializeField] int m_konpeitouNum = 10;

    public int MaxHp => m_maxHp;
    public int KonpeitouNum => m_konpeitouNum;
}
