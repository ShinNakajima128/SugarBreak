using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    string m_weaponName = "";

    [SerializeField]
    Sprite m_activeWeaponImage = default;

    [SerializeField]
    Sprite m_deactiveWeaponImage = default;

    public string WeaponName { get => m_weaponName; }

    public Sprite ActiveWeaponImage { get => m_activeWeaponImage; }

    public Sprite DeactiveWeaponImage { get => m_deactiveWeaponImage; }
}
