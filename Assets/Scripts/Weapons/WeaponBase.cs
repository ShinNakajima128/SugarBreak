using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]protected int attackDamage = 5;

    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
}
