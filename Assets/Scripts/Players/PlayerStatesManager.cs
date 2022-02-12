using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class PlayerStatesManager : MonoBehaviour, IDamagable
{
    [Header("プレイヤーのデータ")]
    [SerializeField] 
    PlayerData m_playerData = default;

    /// <summary> 金平糖の所持数を表示するテキスト </summary>
    TextMeshProUGUI m_totalKonpeitouTmp = default;

    HpGauge m_hpGauge = default;

    /// <summary>  </summary>
    Animator m_anim = default;

    CinemachineFreeLook m_freeLook = default;

    Rigidbody m_rb = default;

    int defaultHp = 8;

    bool isDying = false;

    public static PlayerStatesManager Instance { get; private set; }

    public bool IsOperation { get; set; } = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        m_rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        m_freeLook = GameObject.FindGameObjectWithTag("DefaultCamera").GetComponent<CinemachineFreeLook>();
        m_totalKonpeitouTmp = GameObject.FindGameObjectWithTag("KonpeiText").GetComponent<TextMeshProUGUI>();
        m_hpGauge = GameObject.FindGameObjectWithTag("HPGauge").GetComponent<HpGauge>();
        m_playerData.SetStartHp(defaultHp);
        m_hpGauge.SetHpGauge(m_playerData.HP);
        GameManager.GameEnd += OffOperation;
        EventManager.ListenEvents(Events.GetKonpeitou, UpdateCount);
        m_totalKonpeitouTmp.text = m_playerData.TotalKonpeitou.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(1);
            m_hpGauge.SetHpGauge(m_playerData.HP);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Damage(1);
            m_hpGauge.SetHpGauge(m_playerData.HP);
        }
    }

    public void Damage(int attackPower)
    {
        if (isDying || PlayerController.Instance.IsDodged) return;

        m_playerData.HP -= attackPower;
        m_hpGauge.SetHpGauge(m_playerData.HP);

        if (m_playerData.HP <= 0)
        {
            isDying = true;
            StartCoroutine(Dying());

            if (BossArea.isBattle) BossArea.isBattle = false;
        }
        else
        {
            SoundManager.Instance.PlaySeByName("Damage3");
            m_anim.SetTrigger("isDamaged");
        }
    }

    public void Heal(int healValue)
    {
        if (isDying) return;

        m_playerData.HP += healValue;
        m_hpGauge.SetHpGauge(m_playerData.HP);

        if (m_playerData.HP > m_playerData.MaxHp)
        {
            m_playerData.HP = m_playerData.MaxHp;
        }
    }

    public void OffOperation()
    {
        IsOperation = false;
        m_anim.SetFloat("Move", 0f);
        m_rb.velocity = Vector3.zero;
        m_rb.constraints = RigidbodyConstraints.FreezeAll;
        m_freeLook.m_XAxis.m_InputAxisName = "";
        m_freeLook.m_YAxis.m_InputAxisName = "";
    }

    public void OnOperation()
    {
        IsOperation = true;
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        m_freeLook.m_XAxis.m_InputAxisName = "Camera X";
        m_freeLook.m_YAxis.m_InputAxisName = "Camera Y";
    }

    IEnumerator Dying()
    {
        IsOperation = false;
        m_anim.SetFloat("Move", 0f);
        m_rb.velocity = Vector3.zero;
        m_anim.Play("Dying");
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySeByName("Gameover");
        SoundManager.Instance.PlayVoiceByName("univ1077");
        yield return new WaitForSeconds(2.8f);

        LoadSceneManager.Instance.FadeIn(LoadSceneManager.Instance.Masks[3]);　//フェード開始
        m_freeLook.m_XAxis.m_InputAxisName = "";
        m_freeLook.m_YAxis.m_InputAxisName = "";

        yield return new WaitForSeconds(1.0f);

        LoadSceneManager.Instance.LoadAnim.SetActive(true); //ロード画面
        
        yield return new WaitForSeconds(3.0f);
        SoundManager.Instance.PlayBgmByName("BakeleValley1");
        ReturnArea.Instance.ReturnComebackPoint(); //復帰地点に戻る
        m_freeLook.m_XAxis.m_InputAxisName = "Camera X";
        m_freeLook.m_YAxis.m_InputAxisName = "Camera Y";
        m_playerData.HP = 8;                         //体力リセット
        m_hpGauge.SetHpGauge(m_playerData.HP);
        isDying = false;
    }

    /// <summary>
    /// 金平糖の所持数を更新する
    /// </summary>
    void UpdateCount()
    {
        m_totalKonpeitouTmp.text = m_playerData.TotalKonpeitou.ToString();
    }
}
