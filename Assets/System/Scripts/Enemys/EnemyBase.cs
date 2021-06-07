using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] int m_maxHp = 10;
    [SerializeField] Slider m_HpSlider = null;
    int currentHp;

    void Start()
    {
        m_HpSlider.maxValue = m_maxHp;
        currentHp = m_maxHp;
        m_HpSlider.value = currentHp;
    }

    public void Damage(int attackPower)
    {
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
