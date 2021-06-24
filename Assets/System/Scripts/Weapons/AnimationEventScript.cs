using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationEventScript : MonoBehaviour
{
    [SerializeField] GameObject[] m_weaponList = null;
    Dictionary<string, int> weaponIndex = new Dictionary<string, int>();
    SoundManager soundManager;
    public WeaponState weaponStates = WeaponState.PopLauncher;
    public bool isChanged = false;
    public event Action AttackAction;

    public enum WeaponState
    {
        CandyBeat,
        PopLauncher
    }

    private void Awake()
    {
        for (int i = 0; i < m_weaponList.Length; i++)
        {
            weaponIndex.Add(m_weaponList[i].name, i);
        }
    }

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        switch (weaponStates)
        {
            case WeaponState.CandyBeat:
                //if (!isChanged)
                //{
                //    m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(true);
                //    m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(false);
                //    isChanged = true;
                //}
                m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(true);
                m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(false);
                break;
            case WeaponState.PopLauncher:
                //if (!isChanged)
                //{
                //    m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(false);
                //    m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(true);
                //    isChanged = true;
                //}
                m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(false);
                m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(true);
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

    public void CandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat"); 
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = true;
        soundManager.PlaySeByName("JumpAttack");
    }

    public void FinishCandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat");
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = false;
    }

    public void LightCandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat");
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = true;
        soundManager.PlaySeByName("LightAttack");
    }
    public void FinishLightCandyAttack()
    {
        int candyBeat = GetWeaponIndex("CandyBeat");
        m_weaponList[candyBeat].GetComponent<BoxCollider>().enabled = false;
    }

    public void ShootBullet()
    {
        AttackAction();
    }
}
