using SugarBreak;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/// <summary>
/// データの種類
/// </summary>
public enum DataTypes
{
    All,
    Player,
    Option,
}

public class SaveManager
{
    static SaveManager Instance = new SaveManager();

    const string FILEPATH = "SaveData/gamedata.json";

    GameData Data = default;

    async public static void Load()
    {
        Instance.Data = LocalData.Load<GameData>(FILEPATH);

        if (Instance.Data == null)
        {
            Instance.Data = new GameData();
            Instance.Data.Initialize();
            await Instance.WaitProcess();
            Save(DataTypes.All);
            DataManager.Instance.LoadData(Instance.Data);
        }
        else
        {
            Instance.Data.Apply();
            Debug.Log(JsonUtility.ToJson(Instance.Data));
            await Instance.WaitProcess();
            Save(DataTypes.All);
        }
    }

    public static GameData GetData()
    {
        if (Instance.Data == null)
        {
            Load();
        }
        return Instance.Data;
    }

    async public static void Save(DataTypes type)
    {
        UpdateData(type);
        await Instance.WaitProcess();
        Debug.Log("セーブしました");
        LocalData.Save<GameData>(FILEPATH, Instance.Data);
    }

    static void UpdateData(DataTypes type)
    {
        switch (type)
        {
            case DataTypes.All:
                var p = DataManager.Instance.GetPlayerData;
                Instance.Data.PlayerData.UpdateData(p);
                var o = DataManager.Instance.GetOptionData;
                Instance.Data.OptionData.UpdateData(o);
                break;
            case DataTypes.Player:
                var playerData = DataManager.Instance.GetPlayerData;
                Instance.Data.PlayerData.UpdateData(playerData);
                break;
            case DataTypes.Option:
                var optionData = DataManager.Instance.GetOptionData;
                Instance.Data.OptionData.UpdateData(optionData);
                break;
        }
    }
    async UniTask WaitProcess()
    {
        await UniTask.Delay(500);
        
    }
}