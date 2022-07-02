﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    PlayerData _playerData = default;

    [SerializeField]
    OptionData _optionData = default;

    [SerializeField]
    WeaponData[] _allWeaponDatas = default;

    /// <summary>
    /// プレイヤーデータを取得
    /// </summary>
    public PlayerData GetPlayerData => _playerData;
    public OptionData GetOptionData => _optionData;
    public WeaponData[] AllWeaponDatas => _allWeaponDatas;

    public static DataManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// データをロードする
    /// </summary>
    /// <param name="data"> ゲームデータ </param>
    public void LoadData(GameData data)
    {
        _playerData.SetData(data);
        _optionData.SoundOptionData.SetData(data);
    }
}
