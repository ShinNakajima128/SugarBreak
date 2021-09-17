using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("各武器のデータ")]
    [SerializeField]
    WeaponData m_data = default;

    [SerializeField]
    protected int attackDamage = 5;

    public WeaponData WeaponData => m_data;

    public int AttackDamage { get => attackDamage; set => attackDamage = value; }

    public bool IsActived { get; set; }
}
