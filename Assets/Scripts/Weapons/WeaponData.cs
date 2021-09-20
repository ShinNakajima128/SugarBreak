using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("武器名")]
    [SerializeField]
    string m_weaponName = "";

    [Header("使用している時の画像")]
    [SerializeField]
    Sprite m_activeWeaponImage = default;

    [Header("使用していない時の画像")]
    [SerializeField]
    Sprite m_deactiveWeaponImage = default;

    public string WeaponName { get => m_weaponName; }

    public Sprite ActiveWeaponImage { get => m_activeWeaponImage; }

    public Sprite DeactiveWeaponImage { get => m_deactiveWeaponImage; }
}
