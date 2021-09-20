using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class PlayerStatesManager : MonoBehaviour, IDamagable
{
    public static PlayerStatesManager Instance;
    [SerializeField] 
    PlayerData playerData = default;

    [SerializeField] 
    TextMeshProUGUI m_totalKonpeitouTmp = default;

    [SerializeField] 
    HpGauge hpGauge = default;

    [SerializeField] 
    Animator m_anim = default;

    [SerializeField] 
    CinemachineFreeLook m_freeLook = default;

    [SerializeField] 
    Rigidbody m_rb = default;
    int defaultHp = 8;

    bool isDying = false;

    public bool IsOperation { get; set; } = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerData.SetStartHp(defaultHp);
        hpGauge.SetHpGauge(playerData.HP);
        GameManager.GameEnd += OffOperation;
    }

    void Update()
    {
        m_totalKonpeitouTmp.text = playerData.TotalKonpeitou.ToString();

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(1);
            hpGauge.SetHpGauge(playerData.HP);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Damage(1);
            hpGauge.SetHpGauge(playerData.HP);
        }
    }

    public void Damage(int attackPower)
    {
        if (isDying) return;

        playerData.HP -= attackPower;
        hpGauge.SetHpGauge(playerData.HP);

        if (playerData.HP <= 0)
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

        playerData.HP += healValue;
        hpGauge.SetHpGauge(playerData.HP);

        if (playerData.HP > playerData.MaxHp)
        {
            playerData.HP = playerData.MaxHp;
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
        playerData.HP = 8;                         //体力リセット
        hpGauge.SetHpGauge(playerData.HP);
        isDying = false;
    }
}
