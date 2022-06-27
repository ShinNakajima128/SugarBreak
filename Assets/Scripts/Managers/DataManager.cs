using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    PlayerData _playerData = default;

    [SerializeField]
    WeaponData[] _allWeaponDatas = default;

    public WeaponData[] AllWeaponDatas => _allWeaponDatas;

    public static DataManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
}
