using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class WeaponList
{
    public WeaponData[] Weapons = new WeaponData[3];
    public WeaponList(WeaponData first, WeaponData second, WeaponData third)
    {
        Weapons[0] = first;
        Weapons[1] = second;
        Weapons[2] = third;
    }
}

public enum WeaponTypes 
{
    Default,
    CandyBeat,
    PopLauncher,
    DualSoda
}
public class WeaponListControl : MonoBehaviour
{
    [SerializeField]
    WeaponList m_currentEquipWeapons = default;

    [SerializeField]
    WeaponTypes m_currentWeapon = default;

    [SerializeField]
    bool isDebug = false;

    [SerializeField]
    WeaponData[] weapons = default;

    [SerializeField]
    string m_weaponListFileName = "/WeaponList.json";

    string dataPath;

    public static WeaponListControl Instance { get; private set; }

    public Sprite[] CurrentWeaponImages { get => m_currentEquipWeapons.Weapons.Select(s => s.ActiveWeaponImage).ToArray(); }

    private void Awake()
    {
        Instance = this;
#if UNITY_EDITOR
        dataPath = Application.dataPath + m_currentEquipWeapons;
#endif

    }

    private void Start()
    {
        if (isDebug)
        {
            m_currentEquipWeapons = new WeaponList(weapons[0], weapons[1], weapons[2]);
            m_currentWeapon = m_currentEquipWeapons.Weapons[0].WeaponType;
        }
    }

    public void ChangeWeapon(int index)
    {

    }
}
