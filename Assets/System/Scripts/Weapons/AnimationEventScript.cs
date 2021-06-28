using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponState
{
    CandyBeat,
    PopLauncher
}

public class AnimationEventScript : MonoBehaviour
{
    [SerializeField] GameObject[] m_weaponList = null;
    Dictionary<string, int> weaponIndex = new Dictionary<string, int>();
    SoundManager soundManager;
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

    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        switch (weaponStates)
        {
            case WeaponState.CandyBeat:
                
                m_weaponList[GetWeaponIndex("RolipopCandy")].SetActive(true);
                m_weaponList[GetWeaponIndex("PopLauncher")].SetActive(false);
                break;
            case WeaponState.PopLauncher:
                
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
        EffectManager.PlayEffect(EffectType.Slam, this.transform.position);
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
        soundManager.PlaySeByName("Shoot");
    }

    public void FootStep()
    {
        soundManager.PlaySeByName("FootStep");
    }
}
