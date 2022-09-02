using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
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

    [Tooltip("武器作成ボタン")]
    [SerializeField]
    Button _createButton = default;

    [Tooltip("武器配置画面")]
    [SerializeField]
    GameObject _weaponsPlacementPanel = default;

    [Tooltip("武器メニューのボタンをまとめたObject")]
    [SerializeField]
    GameObject _weaponMenuButtonPanel = default;
    #endregion

    #region private
    /// <summary> 現在の金平糖の数 </summary>
    WeaponData _currentSelectWeaponData;
    int _currentSugarPlumNum = 0;
    List<WeaponListButton> _weaponDataList = new List<WeaponListButton>();
    bool _isSetup = false;
    #endregion

    #region public
    /// <summary> モデルの回転処理をまとめたAction </summary>
    public Action<RotateType> OnRotateAction = default;

    /// <summary> アクティブ時に実行する処理をまとめたAction </summary>
    public Action OnActiveAction = default;

    /// <summary> 非アクティブになる時実行する処理をまとめたAction </summary>
    public Action OnDeactiveAction = default;

    /// <summary> 武器リストのボタンを押した時に実行する処理をまとめたAction </summary>
    public Action<WeaponData> OnWeaponButtonClickAction = default;

    /// <summary> 装備ボタンを押した時に実行する処理をまとめたAction </summary>
    public Action OnEquipButtonClickAction = default;

    /// <summary> 外すボタンを押した時に実行する処理をまとめたAction </summary>
    public Action OnRemoveButtonClickAction = default;
    #endregion

    #region property
    public static WeaponMenuManager Instance { get; private set; }
    #endregion

    void OnEnable()
    {
        if (_isSetup)
        {
            OnActiveAction?.Invoke();
        }
    }

    void OnDisable()
    {
        OnDeactiveAction?.Invoke();
    }

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
        _isSetup = true;
    }

    void Update()
    {
        //キャラクターを回転させる
        if (Input.GetKey(KeyCode.Q)|| Input.GetAxis("Trigger") < 0)
        {
            OnRotateAction(RotateType.Left); //左回転
        }
        else if (Input.GetKey(KeyCode.E) || Input.GetAxis("Trigger") > 0)
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
                OnWeaponButtonClickAction?.Invoke(b.WeaponButtonData);
                _currentSelectWeaponData = b.WeaponButtonData;
                //MenuCursor.CursorMove(b.CursorTarget.position);
            });
            _weaponDataList.Add(b);
        }
        ButtonUIController.Instance.FirstButtonList[8] = _weaponDataList[0].GetComponent<Button>();
    }
    #region button actions
    /// <summary>
    /// 武器を配置する画面を表示する
    /// </summary>
    public void Equip()
    {
        _weaponsPlacementPanel.SetActive(true);
        OnEquipButtonClickAction?.Invoke();
    }

    public void Remove()
    {
        OnRemoveButtonClickAction?.Invoke();
        SaveManager.Save(DataTypes.Player);
        SaveManager.Save(DataTypes.Weapon);
    }

    public void Create()
    {
        _currentSelectWeaponData.IsUnrocked = true;

        foreach (var w in _weaponDataList)
        {
            //現在選択している武器データと開放するデータが同じ場合
            if (w.WeaponButtonData.WeaponType == _currentSelectWeaponData.WeaponType)
            {
                WeaponCreate(w.WeaponButtonData.GetRequireCreateKonpeitoNum, () => 
                {
                    w.SetData(_currentSelectWeaponData);
                    ViewData(w.WeaponButtonData);
                    OnWeaponButtonClickAction?.Invoke(w.WeaponButtonData);
                    _equipButton.OnEquipButton(w.WeaponButtonData);
                });
                return;
            }
        }
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
            _createButton.gameObject.SetActive(true);

            //武器素材を獲得している且つ、素材が足りていたらボタンをONにする
            if (data.IsCreating && data.IsGetWeaponMaterial)
            {
                _createButton.interactable = true;
            }
            else
            {
                _createButton.interactable = false;
            }

            if (_weaponMenuButtonPanel.activeSelf)
            {
                _weaponMenuButtonPanel.SetActive(false);
            }
            return;
        }

        //武器作成ボタンがアクティブの場合は非表示にする
        if (_createButton.gameObject.activeSelf)
        {
            _createButton.gameObject.SetActive(false);
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

    void UpdateWeaponButton()
    {

    }

    void WeaponCreate(int consumeNum, Action action = null)
    {
        int resultNum = _currentSugarPlumNum - consumeNum;

        DOTween.To(() => _currentSugarPlumNum,
            (n) => _currentSugarPlumNum = n,
            resultNum,
            1.0f)
            .OnUpdate(() => 
            {
                _sugarPlumText.text = _currentSugarPlumNum.ToString();
            })
            .OnComplete(() => 
            {
                _data.TotalKonpeitou = _currentSugarPlumNum;
                action?.Invoke();
            });
    }
}
