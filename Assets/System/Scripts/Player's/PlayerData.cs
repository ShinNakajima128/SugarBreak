using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] int m_maxHp = 8;
    [SerializeField] int m_hp = 8;
    [SerializeField] int m_totalKonpeitou = 0;

    public int MaxHp
    {
        get { return m_maxHp; }
        set 
        { 
            if (m_maxHp <= 10) m_maxHp = value;
            
            if (m_maxHp > 10)
            {
                m_maxHp = 10;
            }
        }
    }

    public int HP
    {
        get 
        { 
            return m_hp; 
        }
        set 
        { 
            if (m_hp <= m_maxHp) 
            {
                m_hp = value;
            }

            if (m_hp > m_maxHp)
            {
                m_hp = m_maxHp;
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

    public void SetStartHp(int hp)
    {
        m_maxHp = hp;
        m_hp = hp;
    }
}
