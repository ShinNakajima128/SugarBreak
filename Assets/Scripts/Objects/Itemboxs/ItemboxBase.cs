using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemboxBase : MonoBehaviour, IDamagable
{
    [SerializeField] protected ItemboxData itemboxData = default;
    [SerializeField] protected float m_vanishTime = 2.0f;
    protected int m_currentHp;
    protected int m_konpeitouNum;

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

    public virtual void Damage(int attackDamage)
    {
        m_currentHp -= attackDamage;

        if (m_currentHp <= 0)
        {
            KonpeitouGenerator.Instance.GenerateKonpeitou(transform, m_konpeitouNum);
            StartCoroutine(Vanish(EffectType.EnemyDead, m_vanishTime));
        }
    }

    protected IEnumerator Vanish(EffectType effectType, float vanishTime)
    {
        yield return new WaitForSeconds(vanishTime);

        EffectManager.PlayEffect(effectType, transform.position);
        Destroy(this.gameObject);
    }
}
