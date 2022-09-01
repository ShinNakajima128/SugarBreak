using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// ボスのUIを管理するクラス
/// </summary>
public class BossUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_bossUIPanel = default;

    [SerializeField]
    RectTransform m_hpBarObject = default;

    [SerializeField]
    Image m_fillArea = default;

    [SerializeField]
    Image m_delayFillArea = default;

    [SerializeField]
    TextMeshProUGUI m_bossName = default;

    string m_currentBossName;
    int m_currentBossMaxHp;
    int m_currentBossHp;
    

    public static BossUIManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OffUI();
        EventManager.ListenEvents(Events.BossBattleStart, OnUI);
        EventManager.ListenEvents(Events.BossBattleEnd, OffUI);
        EventManager.ListenEvents(Events.OnHUD, OnBossUI);
        EventManager.ListenEvents(Events.OffHUD, OffUI);
    }

    void OnUI()
    {
        StartCoroutine(SetUp());
    }

    void OnBossUI()
    {
        m_bossUIPanel.SetActive(true);
    }

    void OffUI()
    {
        m_bossUIPanel.SetActive(false);
    }

    void StartBossBattle(string bossName, int bossMaxHP)
    {
        m_bossName.text = bossName;
        m_fillArea.fillAmount = bossMaxHP/bossMaxHP;
    }

    /// <summary>
    /// ダメージを受けた時のHPのアニメーション
    /// </summary>
    void HpBarDamageAnimation()
    {
        m_hpBarObject.DOShakePosition(0.2f, 20, 20);
    }

    /// <summary>
    /// ボスがダメージを受けた時の処理
    /// </summary>
    public void DamageHandle(int damageValue)
    {
        HpBarDamageAnimation();
        m_currentBossHp -= damageValue;
        m_fillArea.DOFillAmount((float)m_currentBossHp / m_currentBossMaxHp, 0.2f)
                  .OnComplete(() => 
                  {
                      m_delayFillArea.DOFillAmount((float)m_currentBossHp / m_currentBossMaxHp, 1f);
                  });
        Debug.Log($"現在のHP:{m_currentBossHp},最大HP{m_currentBossMaxHp}, 残り残量：{((float)m_currentBossHp /m_currentBossMaxHp)*100}%");
    }

    IEnumerator SetUp()
    {
        m_bossUIPanel.SetActive(true);

        yield return null;

        m_currentBossName = GameManager.Instance.CurrentBossData.enemyName;
        m_currentBossMaxHp = GameManager.Instance.CurrentBossData.maxHp;
        m_currentBossHp = m_currentBossMaxHp;

        StartBossBattle(m_currentBossName, m_currentBossMaxHp);
    }
}
