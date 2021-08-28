using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerStatesManager : MonoBehaviour, IDamagable
{
    [SerializeField] PlayerData playerData = default;
    [SerializeField] Text m_totalKonpeitou = default;
    [SerializeField] Text m_hpText = default;
    [SerializeField] HpGauge hpGauge = default;
    [SerializeField] SoundManager soundManager = default;
    [SerializeField] Animator m_anim = default;
    [SerializeField] PlayerController m_player = default;
    [SerializeField] CinemachineFreeLook m_freeLook = default;
    [SerializeField] Rigidbody m_rb = default;
    public static bool isOperation = true;
    int defaultHp = 8;

    
    void Start()
    {
        playerData.SetStartHp(defaultHp);
        hpGauge.SetHpGauge(playerData.HP);
    }

    void Update()
    {
        m_hpText.text = playerData.HP.ToString();
        m_totalKonpeitou.text = "× " + playerData.TotalKonpeitou.ToString();

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerData.HP++;
            hpGauge.SetHpGauge(playerData.HP);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            playerData.HP--;
            hpGauge.SetHpGauge(playerData.HP);
        }
    }

    public void Damage(int attackPower)
    {
        playerData.HP -= attackPower;
        hpGauge.SetHpGauge(playerData.HP);
        Debug.Log("被弾");
        soundManager.PlaySeByName("Damage");
        m_anim.SetTrigger("isDamaged");
    }

    public void OffOperation()
    {
        isOperation = false;
        m_anim.SetFloat("Move", 0f);
        m_rb.velocity = Vector3.zero;
        m_freeLook.m_XAxis.m_InputAxisName = "";
        m_freeLook.m_YAxis.m_InputAxisName = "";
    }

    public void OnOperation()
    {
        isOperation = true;
        m_freeLook.m_XAxis.m_InputAxisName = "Camera X";
        m_freeLook.m_YAxis.m_InputAxisName = "Camera Y";
    }
}
