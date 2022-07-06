using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームデータ
/// </summary>
public class GameData
{
    public Player PlayerData;
    public Option OptionData;
    public AllWeapon AllWeaponData;

    public void Initialize()
    {
        PlayerData = new Player();
        OptionData = new Option();
        AllWeaponData = new AllWeapon();

        var player = DataManager.Instance.GetPlayerData;
        var weapons = DataManager.Instance.AllWeaponDatas;

        PlayerData.Init(player.CurrentWeaponList, player.StageData);
        OptionData.Init();
    }
    public void Apply()
    {
        var player = DataManager.Instance.GetPlayerData;
        var option = DataManager.Instance.GetOptionData;

        PlayerData.ApplyData(player);
        OptionData.ApplyData(option);
    }
}

/// <summary>
/// プレイヤーデータ
/// </summary>
[Serializable]
public class Player
{
    public int MaxHp; 
    public int TotalKonpeitou;
    public WeaponList CurrentWeaponList; 
    public Stage[] Stages;
    public bool IsFirstPlay;

    public void Init(WeaponList list, Stage[] stages)
    {
        MaxHp = 8;
        TotalKonpeitou = 0;
        CurrentWeaponList = list;
        Stages = stages;
        IsFirstPlay = false;
    }

    /// <summary>
    /// データを更新する
    /// </summary>
    /// <param name="data"> 新しいデータ </param>
    public void UpdateData(PlayerData data)
    {
        MaxHp = data.MaxHp;
        TotalKonpeitou = data.TotalKonpeitou;
        CurrentWeaponList = data.CurrentWeaponList;
        Stages = data.StageData;
        IsFirstPlay = data.IsFirstPlay;
    }

    public void ApplyData(PlayerData data)
    {
        data.MaxHp = MaxHp;
        data.TotalKonpeitou = TotalKonpeitou;
        data.CurrentWeaponList = CurrentWeaponList;
        data.StageData = Stages;
        data.IsFirstPlay = IsFirstPlay;
    }
}

/// <summary>
/// オプションデータ
/// </summary>
[Serializable]
public class Option
{
    //GraphicParameters Graphic;
    public SoundParameters Volumes;

    public void Init()
    {
        Volumes = new SoundParameters();

        Debug.Log("サウンドオプション初期化");
        Volumes.MasterVolume = 1.0f;
        Volumes.BgmVolume = 0.3f;
        Volumes.SeVolume = 1.0f;
        Volumes.VoiceVolume = 1.0f;
        Volumes.IsMasterMute = false;
        Volumes.IsBgmMute = false;
        Volumes.IsSeMute = false;
        Volumes.IsVoiceMute = false;
    }

    /// <summary>
    /// セーブデータを更新する
    /// </summary>
    /// <param name="data"> 新しいデータ </param>
    public void UpdateData(OptionData data)
    {
        Volumes.MasterVolume = data.SoundOptionData.MasterVolume;
        Volumes.BgmVolume = data.SoundOptionData.BgmVolume;
        Volumes.SeVolume = data.SoundOptionData.SeVolume;
        Volumes.VoiceVolume = data.SoundOptionData.VoiceVolume;
        Volumes.IsMasterMute = data.SoundOptionData.IsMasterMute;
        Volumes.IsBgmMute = data.SoundOptionData.IsBgmMute;
        Volumes.IsSeMute = data.SoundOptionData.IsSeMute;
        Volumes.IsVoiceMute = data.SoundOptionData.IsVoiceMute;
    }

    /// <summary>
    /// セーブデータをゲーム中に扱うデータに反映させる
    /// </summary>
    /// <param name="data"> セーブデータ </param>
    public void ApplyData(OptionData data)
    {
        data.SoundOptionData.MasterVolume = Volumes.MasterVolume;
        data.SoundOptionData.BgmVolume = Volumes.BgmVolume;
        data.SoundOptionData.SeVolume = Volumes.SeVolume;
        data.SoundOptionData.VoiceVolume = Volumes.VoiceVolume;
        data.SoundOptionData.IsMasterMute = Volumes.IsMasterMute;
        data.SoundOptionData.IsBgmMute = Volumes.IsBgmMute;
        data.SoundOptionData.IsSeMute = Volumes.IsSeMute;
        data.SoundOptionData.IsVoiceMute = Volumes.IsVoiceMute;
    }
}

/// <summary>
/// 
/// </summary>
[Serializable]
public class AllWeapon
{
    public WeaponData[] WeaponsData;

    public void Init()
    {
        
    }
}

/// <summary>
/// オプションのグラフィックの各パラメーター
/// </summary>
public struct GraphicParameters
{

}

/// <summary>
/// オプションのサウンドの各パラメーター
/// </summary>
[Serializable]
public struct SoundParameters
{
    public float MasterVolume;
    public float BgmVolume;
    public float SeVolume;
    public float VoiceVolume;
    public bool IsMasterMute;
    public bool IsBgmMute;
    public bool IsSeMute;
    public bool IsVoiceMute;
}
