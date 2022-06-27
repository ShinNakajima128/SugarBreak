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
    [Header("武器の名前")]
    [SerializeField]
    string m_weaponName = "";

    [Header("武器の説明文")]
    [TextArea(1, 10)]
    [SerializeField]
    string m_description = "";

    [Header("武器の種類")]
    [SerializeField]
    WeaponTypes m_weaponType = default;

    [Header("武器を装備しているか")]
    [SerializeField]
    bool m_isEquiped = false;

    [Header("使用している時の画像")]
    [SerializeField]
    Sprite m_activeWeaponImage = default;

    [Header("使用していない時の画像")]
    [SerializeField]
    Sprite m_deactiveWeaponImage = default;

    [SerializeField]
    GameObject m_WeaponObject = default;

    public string WeaponName => m_weaponName;

    public string Description => m_description;

    public WeaponTypes WeaponType => m_weaponType; 
    public Sprite ActiveWeaponImage => m_activeWeaponImage;

    public Sprite DeactiveWeaponImage => m_deactiveWeaponImage;
    public GameObject WeaponObject => m_WeaponObject;
    
    public bool IsEquipped { get => m_isEquiped; set => m_isEquiped = value; } 
}
