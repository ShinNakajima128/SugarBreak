using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 武器メニュー画面の機能を管理するクラス
/// </summary>
public class WeaponMenuManager : MonoBehaviour
{
    #region serialize field
    [Tooltip("プレイヤーデータ")]
    [SerializeField]
    PlayerData _data = default;

    [Header("UIオブジェクト")]
    [Tooltip("拠点のUIを管理するクラス")]
    [SerializeField]
    BaseUI _baseUI = default;

    [Tooltip("武器リストのボタンの親オブジェクト")]
    [SerializeField]
    Transform _weaponListButtonParent = default;

    [Tooltip("金平糖の数を表示するText")]
    [SerializeField]
    TextMeshProUGUI _sugarPlumText = default;

    [Tooltip("武器リストのButton")]
    [SerializeField]
    WeaponListButton _weaponListButtonPrefab = default;
    #endregion

    #region private
    /// <summary> 現在の金平糖の数 </summary>
    int _currentSugarPlumNum = 0;
    #endregion

    #region public
    /// <summary> モデルの回転処理をまとめたAction </summary>
    public Action<RotateType> OnRotateAction = default;
    #endregion

    #region property
    public static WeaponMenuManager Instance { get; private set; }
    #endregion

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _currentSugarPlumNum = _data.TotalKonpeitou;
        _sugarPlumText.text = _currentSugarPlumNum.ToString();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            OnRotateAction(RotateType.Left); //左回転
        }
        else if (Input.GetKey(KeyCode.E))
        {
            OnRotateAction(RotateType.Right); //右回転
        }
    }

    void ListSetup()
    {
        var weaponLists = DataManager.Instance.AllWeaponDatas;
        
        for (int i = 0; i < weaponLists.Length; i++)
        {
            var b = Instantiate(_weaponListButtonPrefab, _weaponListButtonParent);
            b.WeaponButtonData = weaponLists[i];
        }
    }

    /// <summary>
    /// 装備画面に戻る
    /// </summary>
    public void BackEquipmentPanel()
    {
        _baseUI.OnEquipment();
    }
}
