using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    BetterGolem,
    CottonCandy
}

public class ChocoEgg : MonoBehaviour, IDamagable
{
    [SerializeField]
    GameObject[] m_dropItems = default;

    [SerializeField]
    int m_maxHp = 10;

    [SerializeField]
    BossType m_bossType = BossType.BetterGolem;

    [SerializeField]
    BoxCollider m_talkTrigger = default;

    [SerializeField]
    bool isDebug = false;

    [SerializeField]
    PlayerData m_playerData = default;

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

    public void Damage(int attackPower, Rigidbody hitRb = null, Vector3 blowUpDir = default, float blowUpPower = 1)
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
            AudioManager.PlaySE(SEType.Item_GetChocoEgg);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// アイテムを生成する
    /// </summary>
    /// <param name="type"> 倒したボスの種類 </param>
    void GenerateItem(BossType type)
    {
        var boss = GameManager.Instance.CurrentBossData;

        switch (type)
        {
            case BossType.BetterGolem:
                if (!m_playerData.StageData[0].IsStageCleared)
                {
                    Instantiate(m_dropItems[0], transform.position, m_dropItems[0].transform.rotation);
                }
                else
                {
                    ItemGenerator.Instance.GenerateKonpeitou(boss.konpeitou, transform.position);
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
