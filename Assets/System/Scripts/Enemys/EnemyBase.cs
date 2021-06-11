using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] int m_maxHp = 10;
    [SerializeField]protected Slider m_HpSlider = null;
    Animator m_anim;
    protected int currentHp;

    void Start()
    {
        m_HpSlider.maxValue = m_maxHp;
        currentHp = m_maxHp;
        m_HpSlider.value = currentHp;
        m_anim = GetComponent<Animator>();
    }

    public void Damage(int attackPower)
    {
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0)
        {
            m_anim.Play("Die");
            Destroy(this.gameObject, 2.0f);
        }
    }
}
