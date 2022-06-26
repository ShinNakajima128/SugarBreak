using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器の種類
/// </summary>
public enum WeaponTypes
{
    None,
    MainWeapon,
    CandyBeat,
    PopLauncher,
    DualSoda
}

[CreateAssetMenu(menuName = "MyScriptable/Create WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("武器の種類")]
    [SerializeField]
    WeaponTypes m_weaponType = default;

    [Header("使用している時の画像")]
    [SerializeField]
    Sprite m_activeWeaponImage = default;

    [Header("使用していない時の画像")]
    [SerializeField]
    Sprite m_deactiveWeaponImage = default;

    [SerializeField]
    GameObject m_WeaponObject = default;

    public WeaponTypes WeaponType { get => m_weaponType; }

    public Sprite ActiveWeaponImage { get => m_activeWeaponImage; }

    public Sprite DeactiveWeaponImage { get => m_deactiveWeaponImage; }
    public GameObject WeaponObject { get => m_WeaponObject; }
}
