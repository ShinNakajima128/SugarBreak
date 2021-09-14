using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    Dragon,
    CottonCandy
}

public class ChocoEgg : MonoBehaviour, IDamagable
{
    [SerializeField]
    GameObject[] m_dropItems = default;

    [SerializeField]
    int m_maxHp = 10;

    [SerializeField]
    BossType m_bossType = BossType.Dragon;

    [SerializeField]
    BoxCollider m_talkTrigger = default;

    [SerializeField]
    bool isDebug = false;

    int m_currentHp;
    CapsuleCollider m_collider;


    public BossType BossTypes { get; set; }

    void Start()
    {
        m_collider = GetComponent<CapsuleCollider>();
        m_talkTrigger.enabled = false;
        m_collider.enabled = false;
        m_currentHp = m_maxHp;
        StartCoroutine(DelayEvent());
    }

    public void Damage(int attackPower)
    {
        m_currentHp -= attackPower;

        if (m_currentHp <= 0)
        {
            if (isDebug)
            {
                GenerateItem(m_bossType);
            }
            else
            {
                GenerateItem(BossTypes);
            }

            EffectManager.PlayEffect(EffectType.EnemyDead, transform.position);
            Destroy(gameObject);
        }
    }

    void GenerateItem(BossType type)
    {
        switch (type)
        {
            case BossType.Dragon:
                if (!GameManager.Instance.IsBakeleValleyCleared)
                {
                    Instantiate(m_dropItems[0], transform.position, m_dropItems[0].transform.rotation);
                }
                else
                {
                    KonpeitouGenerator.Instance.GenerateKonpeitou(30, transform.position);
                    GameManager.Instance.OnGameEndClearedStage();
                }
                break;
            case BossType.CottonCandy:
                Instantiate(m_dropItems[1], transform.position, m_dropItems[1].transform.rotation);
                break;
            default:
                break;
        }
    }

    IEnumerator DelayEvent()
    {
        yield return new WaitForSeconds(3.0f);

        m_talkTrigger.enabled = true;
        m_collider.enabled = true;
    }
}
