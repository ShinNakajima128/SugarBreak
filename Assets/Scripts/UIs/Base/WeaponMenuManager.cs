using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SugarBreak;

/// <summary>
/// 武器メニュー画面の機能を管理するクラス
/// </summary>
public class WeaponMenuManager : MonoBehaviour
{
    #region serialize field
    [Tooltip("プレイヤーデータ")]
    [SerializeField]
    PlayerData _data = default;

    [Header("UIObjects")]
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

    [Tooltip("武器名を表示するText")]
    [SerializeField]
    TextMeshProUGUI _weaponNameText = default;

    [Tooltip("武器の説明文を表示するText")]
    [SerializeField]
    TextMeshProUGUI _descriptionText = default;

    [Tooltip("装備ボタン")]
    [SerializeField]
    WeaponMenuButton _equipButton = default;

    [Tooltip("強化ボタン")]
    [SerializeField]
    WeaponMenuButton _enhanceButton = default;

    [Tooltip("武器配置画面")]
    [SerializeField]
    GameObject _weaponsPlacementPanel = default;

    [Tooltip("武器メニューのボタンをまとめたObject")]
    [SerializeField]
    GameObject _weaponMenuButtonPanel = default;
    #endregion

    #region private
    /// <summary> 現在の金平糖の数 </summary>
    int _currentSugarPlumNum = 0;
    List<WeaponListButton> _weaponDataList = new List<WeaponListButton>();
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
        StartCoroutine(ListSetup());
        _weaponMenuButtonPanel.SetActive(false);
    }

    void Update()
    {
        //キャラクターを回転させる
        if (Input.GetKey(KeyCode.Q))
        {
            OnRotateAction(RotateType.Left); //左回転
        }
        else if (Input.GetKey(KeyCode.E))
        {
            OnRotateAction(RotateType.Right); //右回転
        }
    }

    /// <summary>
    /// 武器リストのセットアップ
    /// </summary>
    /// <returns></returns>
    IEnumerator ListSetup()
    {
        var weaponLists = DataManager.Instance.AllWeaponDatas;
        
        for (int i = 0; i < weaponLists.Length; i++)
        {
            var b = Instantiate(_weaponListButtonPrefab, _weaponListButtonParent);
            
            yield return null;

            b.SetData(weaponLists[i]);

            b.Click += (() => 
            {
                ViewData(b.WeaponButtonData);
                _equipButton.OnEquipButton(b.WeaponButtonData);
            });
            _weaponDataList.Add(b);
        }
    }
    #region button actions
    /// <summary>
    /// 武器を配置する画面を表示する
    /// </summary>
    public void Equip()
    {
        _weaponsPlacementPanel.SetActive(true);
    }
    /// <summary>
    /// 武器を強化する
    /// </summary>
    public void Enhance()
    {

    }
    /// <summary>
    /// 装備画面に戻る
    /// </summary>
    public void BackEquipmentPanel()
    {
        _baseUI.OnEquipment();
    }
    #endregion

    /// <summary>
    /// 武器データをUIに表示する
    /// </summary>
    /// <param name="data"></param>
    void ViewData(WeaponData data)
    {
        if (!data.IsUnrocked)
        {
            _weaponNameText.text = "？？？";
            _descriptionText.text = "まだ解放されていません";


            if (_weaponMenuButtonPanel.activeSelf)
            {
                _weaponMenuButtonPanel.SetActive(false);
            }
            return;
        }

        _weaponNameText.text = data.WeaponName;
        _descriptionText.text = data.Description;

        if (!_weaponMenuButtonPanel.activeSelf)
        {
            _weaponMenuButtonPanel.SetActive(true);
        }

        if (data.IsEquipped)
        {

        }
    }
}
