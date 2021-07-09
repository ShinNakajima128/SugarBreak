using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] int m_hp = 8;
    [SerializeField] int m_totalKonpeitou = 0;

    public int HP
    {
        get 
        { 
            return m_hp; 
        }
        set 
        { 
            if (m_hp <= 8) 
            {
                m_hp = value;
            }

            if (m_hp > 8)
            {
                m_hp = 8;
            }

            if (m_hp <= 0)
            {
                m_hp = 0;
            }
        }
    }

    public int TotalKonpeitou
    {
        get { return m_totalKonpeitou; }
        set { m_totalKonpeitou = value; }
    }
}
