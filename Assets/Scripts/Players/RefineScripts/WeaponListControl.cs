using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

/// <summary>
/// 武器の装備リストクラス
/// </summary>
[Serializable]
public class WeaponList
{
    public WeaponData Weapon1;
    public WeaponData Weapon2;
    public WeaponData Weapon3;
    public WeaponData MainWeapon;
    public WeaponList(WeaponData first, WeaponData second, WeaponData third, WeaponData main)
    {
        Weapon1 = first;
        Weapon2 = second;
        Weapon3 = third;
        MainWeapon = main;
    }
}

/// <summary>
/// 装備している武器のリストの種類
/// </summary>
public enum WeaponListTypes
{
    Equip1,
    Equip2,
    Equip3,
    MainWeapon,
}

/// <summary>
/// 武器の装備状況を管理するクラス
/// </summary>
public class WeaponListControl : MonoBehaviour
{
    [Tooltip("武器切り替え可能になるまでの時間")]
    [SerializeField]
    float _interval = 1.0f;

    [Tooltip("武器オブジェクト。生成用")]
    [SerializeField]
    GameObject[] _weaponObjects = default;

    [Tooltip("武器アイコンリスト")]
    [SerializeField]
    Image[] _weaponIcons = default;

    [Header("デバッグ用")]
    [SerializeField]
    bool _isDebug = false;

    [Tooltip("現在の武器リスト")]
    [SerializeField]
    WeaponList _currentEquipWeapons = default;

    [Tooltip("現在アクティブの武器")]
    [SerializeField]
    WeaponListTypes _currentWeapon = default;

    [Tooltip("武器のデータ(確認用)")]
    [SerializeField]
    WeaponData[] _debugWeaponDatas = default;

    [Tooltip("プレイヤーデータ")]
    [SerializeField]
    PlayerData _data = default;

    [SerializeField]
    string _weaponListFileName = "WeaponList";

    [Header("UIObjects")]
    [SerializeField]
    GameObject _weaponListPanel = default;

    #region private
    Dictionary<WeaponListTypes, GameObject> _weaponListDic = new Dictionary<WeaponListTypes, GameObject>();
    Dictionary<WeaponListTypes, WeaponData> _weaponDataDic = new Dictionary<WeaponListTypes, WeaponData>();
    Dictionary<WeaponListTypes, Image> _weaponIconsDic = new Dictionary<WeaponListTypes, Image>();

    Transform _weaponListTrans = default;
    bool _isChanged = false;
    bool _init = false;
    #endregion

    #region property
    public static WeaponListControl Instance { get; private set; }

    public WeaponData CurrentWeapon1Data { get => _currentEquipWeapons.Weapon1; }
    public WeaponData CurrentWeapon2Data { get => _currentEquipWeapons.Weapon2; }
    public WeaponData CurrentWeapon3Data { get => _currentEquipWeapons.Weapon3; }
    public WeaponData MainWeaponData { get => _currentEquipWeapons.MainWeapon; }
    public WeaponListTypes CurrentEquipWeapon { get => _currentWeapon; }
    #endregion

    void OnEnable()
    {
        if (!_init)
        {
            StartCoroutine(SetUpCoroutine());
            _init = true;
        }
        _isChanged = false;
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.ListenEvents(Events.OnHUD, OnWeaponListPanel);
        EventManager.ListenEvents(Events.OffHUD, OffWeaponListPanel);
    }

    /// <summary>
    /// 武器リストを表示する
    /// </summary>
    void OnWeaponListPanel()
    {
        _weaponListPanel.SetActive(true);
    }

    /// <summary>
    /// 武器リストを非表示にする
    /// </summary>
    void OffWeaponListPanel()
    {
        _weaponListPanel.SetActive(false);
    }

    /// <summary>
    /// 武器データのセットアップ
    /// </summary>
    void Setup()
    {
        //武器オブジェクトの登録
        //生成したオブジェクトとアニメーションのオブジェクト名と一致させるために(clone)を削除する処理を行う
        //var g1 = m_weaponObjects.FirstOrDefault(o => o.name == m_currentEquipWeapons.Weapon1.WeaponType.ToString());
        var g1 = _currentEquipWeapons.Weapon1.WeaponObject;
        _weaponListDic[WeaponListTypes.Equip1] = Instantiate(g1, _weaponListTrans);
        _weaponListDic[WeaponListTypes.Equip1].name = g1.name;

        var g2 = _currentEquipWeapons.Weapon2.WeaponObject;
        _weaponListDic[WeaponListTypes.Equip2] = Instantiate(g2, _weaponListTrans);
        _weaponListDic[WeaponListTypes.Equip2].name = g2.name;

        var g3 = _currentEquipWeapons.Weapon3.WeaponObject;
        _weaponListDic[WeaponListTypes.Equip3] = Instantiate(g3, _weaponListTrans);
        _weaponListDic[WeaponListTypes.Equip3].name = g3.name;

        var g4 = _currentEquipWeapons.MainWeapon.WeaponObject;
        _weaponListDic[WeaponListTypes.MainWeapon] = Instantiate(g4, _weaponListTrans);
        _weaponListDic[WeaponListTypes.MainWeapon].name = g4.name;

        //武器データの登録
        _weaponDataDic[WeaponListTypes.Equip1] = CurrentWeapon1Data;
        _weaponDataDic[WeaponListTypes.Equip2] = CurrentWeapon2Data;
        _weaponDataDic[WeaponListTypes.Equip3] = CurrentWeapon3Data;
        _weaponDataDic[WeaponListTypes.MainWeapon] = MainWeaponData;

        //武器アイコンの登録
        _weaponIconsDic[WeaponListTypes.Equip1] = _weaponIcons[0];
        _weaponIconsDic[WeaponListTypes.Equip2] = _weaponIcons[1];
        _weaponIconsDic[WeaponListTypes.Equip3] = _weaponIcons[2];
        _weaponIconsDic[WeaponListTypes.MainWeapon] = _weaponIcons[3];
    }

    /// <summary>
    /// 武器を切り替える
    /// </summary>
    /// <param name="type"> 武器リストの種類 </param>
    public void ChangeWeapon(WeaponListTypes type, Action action = null)
    {
        //武器を装備していないボタンを押す、既に装備中、変更後一定時間経過していなければ何もしない
        if (_currentWeapon == type || _isChanged || _weaponDataDic[type].WeaponType == WeaponTypes.None)
        {
            Debug.Log($"武器変更不可。現在の武器:{_currentWeapon}, {_isChanged}");
            return;
        }

        //現在の装備状況を更新する
        _currentWeapon = type;

        _isChanged = true;
        StartCoroutine(ChangeInterval());

        action?.Invoke();

        foreach (var w in _weaponListDic)
        {
            if (w.Key == _currentWeapon)
            {
                w.Value.SetActive(true); //武器オブジェクトをアクティブにする
                PlayerController.Instance.CurrentWeaponAction = w.Value.GetComponent<IWeapon>(); //装備中の武器の機能を登録
                _weaponIconsDic[w.Key].sprite = _weaponDataDic[w.Key].ActiveWeaponImage; //武器アイコンをアクティブの画像に差し替える
            }
            else
            {
                w.Value.SetActive(false); //武器オブジェクトを非アクティブにする
                _weaponIconsDic[w.Key].sprite = _weaponDataDic[w.Key].DeactiveWeaponImage; //武器アイコンを非アクティブの画像に差し替える
            }
        }
    }

    /// <summary>
    /// 武器データ読込のコルーチン
    /// </summary>
    IEnumerator SetUpCoroutine()
    {
        //各武器を子オブジェクトとして管理するオブジェクトを取得
        _weaponListTrans = GameObject.FindGameObjectWithTag("WeaponList").transform;

        if (!_isDebug)
        {
            if (PlayerPrefs.HasKey(_weaponListFileName))
            {
                _currentEquipWeapons = JsonUtility.FromJson<WeaponList>(PlayerPrefs.GetString(_weaponListFileName));
                Debug.Log("武器リストのデータを読み込みました");
            }   
        }
        else
        {
            _currentEquipWeapons = new WeaponList(_debugWeaponDatas[0], _debugWeaponDatas[1], _debugWeaponDatas[2], _debugWeaponDatas[3]);
            var debugJson = JsonUtility.ToJson(_currentEquipWeapons);
            PlayerPrefs.SetString(_weaponListFileName, debugJson);
            Debug.Log("武器リストのデータを作成しました");
        }
        Setup();
        yield return null;
        ChangeWeapon(WeaponListTypes.MainWeapon);
        EventManager.OnEvent(Events.RebindWeaponAnimation);
    }

    /// <summary>
    /// 武器切り替えのインターバル
    /// </summary>
    IEnumerator ChangeInterval()
    {
        float timer = 0;

        while (timer <= _interval)
        {
            timer += Time.deltaTime;
            Debug.Log("インターバル中");
            yield return null;
        }
        Debug.Log("武器変更可能");
        _isChanged = false;
    }
}
