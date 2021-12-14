using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponList
{
    public WeaponData[] Weapons;
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

    public static WeaponListControl Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeWeapon(int index)
    {

    }
}
