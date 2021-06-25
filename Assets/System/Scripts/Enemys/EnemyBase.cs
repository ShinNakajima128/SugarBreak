using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.System.Scripts.Enemys;

public class EnemyBase : MonoBehaviour, IDamagable
{
    [SerializeField] int m_maxHp = 10;
    [SerializeField] protected Slider m_HpSlider = null;
    [SerializeField]protected KonpeitouGenerator generator = null;
    protected int m_dropNum = 10;
    protected Animator m_anim;
    protected int currentHp;

    void Awake()
    {
        generator = GameObject.FindGameObjectWithTag("KonpeitoGenerator").GetComponent<KonpeitouGenerator>();
        m_HpSlider.maxValue = m_maxHp;
        currentHp = m_maxHp;
        m_HpSlider.value = currentHp;
        m_anim = GetComponent<Animator>();
    }

    public virtual void Damage(int attackPower)
    {
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0)
        {
            m_anim.Play("Die");
            generator.GenerateKonpeitou(this.transform, m_dropNum);
            Destroy(this.gameObject, 2.0f);
        }
    }
}
