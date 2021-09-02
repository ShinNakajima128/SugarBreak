﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBase : MonoBehaviour, IDamagable
{
    [SerializeField] protected EnemyData enemyData = null;
    [SerializeField] protected Slider m_HpSlider = null;
    [SerializeField] protected float m_vanishTime = 2.0f;
    [SerializeField] protected Transform m_effectPos = null;
    protected int m_dropNum = 10;
    protected Animator m_anim;
    protected int currentHp;
    protected bool isdead = false;

    void Awake()
    {
        m_HpSlider.maxValue = enemyData.maxHp;
        currentHp = enemyData.maxHp;
        m_HpSlider.value = currentHp;
        m_anim = GetComponent<Animator>();
    }

    public virtual void Damage(int attackPower)
    {
        EffectManager.PlayEffect(EffectType.Damage, m_effectPos.position);
        currentHp -= attackPower;
        m_HpSlider.value = currentHp;

        if (currentHp <= 0)
        {
            m_anim.Play("Die");
            KonpeitouGenerator.Instance.GenerateKonpeitou(this.transform, m_dropNum, 2);
            StartCoroutine(Vanish(EffectType.EnemyDead, m_vanishTime));
        }
    }

    protected IEnumerator Vanish(EffectType effectType, float vanishTime)
    {
        yield return new WaitForSeconds(vanishTime);

        EffectManager.PlayEffect(effectType, m_effectPos.position);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 敵のデータを確認する
    /// </summary>
    void ShowData()
    {
        Debug.Log("最大HP：" + enemyData.maxHp +
                  "攻撃力：" + enemyData.atk +
                  "所持金平糖：" + enemyData.konpeitou);
    }
}