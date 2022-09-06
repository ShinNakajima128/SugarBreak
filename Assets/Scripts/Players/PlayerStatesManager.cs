using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using DG.Tweening;

public class PlayerStatesManager : MonoBehaviour, IDamagable
{
    [Header("プレイヤーのデータ")]
    [SerializeField] 
    PlayerData m_playerData = default;

    [SerializeField]
    float m_knockbackTime = 0.5f;

    /// <summary> 金平糖の所持数を表示するテキスト </summary>
    TextMeshProUGUI m_totalKonpeitouTmp = default;

    HpGauge m_hpGauge = default;

    /// <summary>  </summary>
    Animator m_anim = default;

    CinemachineFreeLook m_freeLook = default;

    Rigidbody m_rb = default;

    int defaultHp = 8;

    bool isDying = false;
    bool m_debugMode = false;
    Vector2 m_originTmpPos;

    public static PlayerStatesManager Instance { get; private set; }
    public bool IsOperation { get; set; } = true;
    public bool IsDying => isDying;
    public Transform EffectTarget { get => transform; }

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
        m_originTmpPos = m_totalKonpeitouTmp.gameObject.transform.localPosition;
        if (GameManager.Instance.DebugMode)
        {
            m_debugMode = true;
        }
    }

    void Update()
    {
        if (!m_debugMode)
        {
            return;
        }

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

    public void Damage(int attackPower, Rigidbody hitRb = null, Vector3 blowUpDir = default, float blowUpPower = 1)
    {
        if (isDying || PlayerController.Instance.IsDodged) return;

        m_playerData.HP -= attackPower;
        m_hpGauge.SetHpGauge(m_playerData.HP);

        if (hitRb != null)
        {
            hitRb.AddForce(blowUpDir * -blowUpPower, ForceMode.Impulse);
        }

        if (m_playerData.HP <= 0)
        {
            isDying = true;
            StartCoroutine(Dying());

            if (BossArea.isBattle) BossArea.isBattle = false;
        }
        else
        {
            AudioManager.PlayVOICE(VOICEType.Damage);
            m_anim.SetTrigger("isDamaged");
            StartCoroutine(Damage());
        }
        VibrationController.OnVibration(Strength.Low, 0.3f);
    }

    /// <summary>
    /// 回復する
    /// </summary>
    /// <param name="healValue"> 回復する値 </param>
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

    /// <summary>
    /// 操作不可にする
    /// </summary>
    public void OffOperation()
    {
        IsOperation = false;
        m_anim.SetFloat("Move", 0f);
        m_rb.velocity = Vector3.zero;
        //m_rb.constraints = RigidbodyConstraints.FreezeAll;
        m_rb.Sleep();
        //m_rb.isKinematic = true;
        m_freeLook.m_XAxis.m_InputAxisName = "";
        m_freeLook.m_YAxis.m_InputAxisName = "";
    }

    /// <summary>
    /// 操作可能にする
    /// </summary>
    public void OnOperation()
    {
        IsOperation = true;
        //m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        m_rb.WakeUp();
        m_freeLook.m_XAxis.m_InputAxisName = "Camera X";
        m_freeLook.m_YAxis.m_InputAxisName = "Camera Y";
    }

    /// <summary>
    /// 倒された
    /// </summary>
    IEnumerator Dying()
    {
        IsOperation = false;
        m_anim.SetFloat("Move", 0f);
        m_rb.velocity = Vector3.zero;
        m_anim.Play("Dying");
        AudioManager.StopBGM();
        AudioManager.PlayBGM(BGMType.Gameover, false);
        AudioManager.PlayVOICE(VOICEType.Gameover);
        yield return new WaitForSeconds(2.8f);

        LoadSceneManager.Instance.FadeIn(LoadSceneManager.Instance.Masks[3]);　//フェード開始
        m_freeLook.m_XAxis.m_InputAxisName = "";
        m_freeLook.m_YAxis.m_InputAxisName = "";

        yield return new WaitForSeconds(1.0f);

        LoadSceneManager.Instance.LoadAnim.SetActive(true); //ロード画面
        
        yield return new WaitForSeconds(2.0f);
        AudioManager.PlayBGM(BGMType.BakeleValley_Main);
        ReturnArea.Instance.ReturnComebackPoint(); //復帰地点に戻る
        m_freeLook.m_XAxis.m_InputAxisName = "Camera X";
        m_freeLook.m_YAxis.m_InputAxisName = "Camera Y";
        m_playerData.HP = 8;                         //体力リセット
        m_hpGauge.SetHpGauge(m_playerData.HP);
        isDying = false;
    }

    /// <summary>
    /// 攻撃を受けた時のコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator Damage()
    {
        IsOperation = false;

        yield return new WaitForSeconds(m_knockbackTime);

        IsOperation = true;
    }

    /// <summary>
    /// 金平糖の所持数を更新する
    /// </summary>
    void UpdateCount()
    {
        m_totalKonpeitouTmp.text = m_playerData.TotalKonpeitou.ToString();
        m_totalKonpeitouTmp.gameObject.transform.DOShakePosition(0.1f);
        m_totalKonpeitouTmp.gameObject.transform.DOScale(1.4f, 0.05f)
                                                .OnComplete(() =>
                                                {
                                                    m_totalKonpeitouTmp.gameObject.transform.DOScale(1.0f, 0.05f);
                                                    m_totalKonpeitouTmp.gameObject.transform.localPosition = m_originTmpPos;
                                                });                         
    }
}
