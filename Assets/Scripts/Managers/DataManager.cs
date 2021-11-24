using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /// <summary>
    /// ゲームデータをセーブする
    /// </summary>
    //public static void SaveData()
    //{
    //    SaveData data = SaveManager.GetData();
    //    GameDataObject gameData = FindObjectOfType<GameDataObject>();

    //    ISave saveIf = gameData.GetComponent<ISave>();
    //    saveIf.Save(data.CurrentGameData);
    //    Debug.Log(data.CurrentGameData);
    //    SaveManager.Save();
    //}

    ///// <summary>
    ///// ゲームデータをロードする
    ///// </summary>
    //public static void LoadData()
    //{
    //    SaveManager.Load();
    //    SaveData data = SaveManager.GetData();

    //    var gameData = FindObjectOfType<GameDataObject>();

    //    ISave saveIf = gameData.GetComponent<ISave>();
    //    saveIf.Load(data.CurrentGameData);
    //    Debug.Log(data);
    //}
}
