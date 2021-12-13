using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponList
{
    public WeaponBase[] Weapons;
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
    WeaponList m_CurrentEquipWeapons = default;

    [SerializeField]
    WeaponTypes m_currentWeapon = default;
}
