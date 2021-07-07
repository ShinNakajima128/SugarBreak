using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.System.Scripts.Damage;

public class PlayerStatesManager : MonoBehaviour, IDamagable
{
    [SerializeField] PlayerData playerData = default;
    [SerializeField] Text m_totalKonpeitou = default;
    [SerializeField] Text m_hpText = default;
    [SerializeField] HpGauge hpGauge = default;
    [SerializeField] SoundManager soundManager = default;
    [SerializeField] Animator m_anim = default;
    public static bool isOperation = true;

    
    void Start()
    {
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
        soundManager.PlaySeByName("Enda");
        m_anim.SetTrigger("isDamaged");
    }
}
