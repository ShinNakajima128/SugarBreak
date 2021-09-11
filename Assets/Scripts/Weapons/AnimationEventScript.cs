using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponState
{
    CandyBeat,
    PopLauncher,
    DualSoda
}

/// <summary>
/// 武器のアニメーションイベントの機能を持つクラス
/// </summary>
public class AnimationEventScript : MonoBehaviour
{
    [SerializeField] GameObject[] m_weaponList = null;
    [SerializeField] Transform m_candyBeatEffectPos = null;
    Dictionary<string, int> weaponIndex = new Dictionary<string, int>();
    public WeaponState weaponStates = WeaponState.PopLauncher;
    public bool isChanged = false;
    public event Action AttackAction;

    private void Awake()
    {
        for (int i = 0; i < m_weaponList.Length; i++)
        {
            weaponIndex.Add(m_weaponList[i].name, i);
        }
    }

    void Update()
    {
        switch (weaponStates)
        {
            case WeaponState.CandyBeat:
                m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(true);
                m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(false);
                m_weaponList[GetWeaponIndex("DualSoda")].SetActive(false);
                break;
            case WeaponState.PopLauncher:
                m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(false);
                m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(true);
                m_weaponList[GetWeaponIndex("DualSoda")].SetActive(false);
                break;
            case WeaponState.DualSoda:
                m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(false);
                m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(false);
                m_weaponList[GetWeaponIndex("DualSoda")].SetActive(true);
                break;
        }
    }

    public int GetWeaponIndex(string name)
    {
        if (weaponIndex.ContainsKey(name))
        {
            return weaponIndex[name];
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// キャンディビートのジャンプ攻撃の当たり判定の開始イベント
    /// </summary>
    public void CandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat");
        m_weaponList[candyBeat].GetComponent<CandyBeat>().AttackDamage = 10;
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = true;
        EffectManager.PlayEffect(EffectType.Slam, m_candyBeatEffectPos.position);
        SoundManager.Instance.PlaySeByName("JumpAttack");
    }

    /// <summary>
    /// ジャンプ攻撃の当たり判定の終了イベント
    /// </summary>
    public void FinishCandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat");
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = false;
    }

    public void LightCandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat");
        m_weaponList[candyBeat].GetComponent<CandyBeat>().AttackDamage = 5;
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = true;
        SoundManager.Instance.PlaySeByName("LightAttack");
    }
    public void FinishLightCandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat");
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = false;
    }

    public void ShootBullet()
    {
        AttackAction?.Invoke();
        SoundManager.Instance.PlaySeByName("Shoot");
    }

    public void SwordAttack1()
    {
        int dualsoda = GetWeaponIndex("DualSoda");
        m_weaponList[dualsoda].GetComponent<DualSoda>().AttackDamage = 2;
        m_weaponList[dualsoda].GetComponent<BoxCollider>().enabled = true;
        SoundManager.Instance.PlaySeByName("DualSodaAttack");
    }

    public void FinishSwordAttack1()
    {
        int dualsoda = GetWeaponIndex("DualSoda");
        m_weaponList[dualsoda].GetComponent<BoxCollider>().enabled = false;
    }

    public void SwordAttack2()
    {
        int dualsoda = GetWeaponIndex("DualSoda");
        m_weaponList[dualsoda].GetComponent<DualSoda>().AttackDamage = 4;
        m_weaponList[dualsoda].GetComponent<BoxCollider>().enabled = true;
        SoundManager.Instance.PlaySeByName("DualSodaAttack");
    }

    public void FinishSwordAttack2()
    {
        int dualsoda = GetWeaponIndex("DualSoda");
        m_weaponList[dualsoda].GetComponent<BoxCollider>().enabled = false;
    }

    public void SwordAttack3()
    {
        int dualsoda = GetWeaponIndex("DualSoda");
        m_weaponList[dualsoda].GetComponent<DualSoda>().AttackDamage = 8;
        var collider = m_weaponList[dualsoda].GetComponent<BoxCollider>();
        collider.enabled = true;
        collider.size = new Vector3(2f, 2f, 2f);
        SoundManager.Instance.PlaySeByName("DualSodaFinish");
        EffectManager.PlayEffect(EffectType.Slam, m_candyBeatEffectPos.position);
    }

    public void FinishSwordAttack3()
    {
        int dualsoda = GetWeaponIndex("DualSoda");
        var collider = m_weaponList[dualsoda].GetComponent<BoxCollider>();
        collider.enabled = false;
        collider.size = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void FootStep()
    {
        SoundManager.Instance.PlaySeByName("FootStep");
    }
}
