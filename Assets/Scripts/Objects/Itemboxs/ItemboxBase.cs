﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemboxBase : MonoBehaviour, IDamagable
{
    [SerializeField] 
    protected ItemboxData itemboxData = default;

    [SerializeField] 
    protected float m_vanishTime = 2.0f;

    protected static int m_playSeCount = 0;
    protected int m_currentHp;
    protected int m_konpeitouNum;

    public Transform EffectTarget { get => transform; }

    void Start()
    {
        if (itemboxData)
        {
            m_currentHp = itemboxData.MaxHp;
            m_konpeitouNum = itemboxData.KonpeitouNum;
        }
        else
        {
            Debug.LogWarning("ItemboxDataがありません");
        }
    }

    public virtual void Damage(int attackPower, Rigidbody hitRb = null, Vector3 blowUpDir = default, float blowUpPower = 1)
    {
        m_currentHp -= attackPower;
    }

    protected IEnumerator Vanish(float vanishTime)
    {
        yield return new WaitForSeconds(vanishTime);
        if (m_playSeCount >= 3) { m_playSeCount = 0; }
        Destroy(this.gameObject);
    }
}
