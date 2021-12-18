using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 武器の装備リストクラス
/// </summary>
[Serializable]
public class WeaponList
{
    //public WeaponData[] Weapons = new WeaponData[4];

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
    float m_interval = 1.0f;

    [Tooltip("武器オブジェクト。生成用")]
    [SerializeField]
    GameObject[] m_weaponObjects = default;

    [Tooltip("武器アイコンリスト")]
    [SerializeField]
    Image[] m_weaponIcons = default;

    [Header("デバッグ用")]
    [SerializeField]
    bool isDebug = false;

    [Tooltip("現在の武器リスト")]
    [SerializeField]
    WeaponList m_currentEquipWeapons = default;

    [Tooltip("現在アクティブの武器")]
    [SerializeField]
    WeaponListTypes m_currentWeapon = default;

    [Tooltip("武器のデータ(確認用)")]
    [SerializeField]
    WeaponData[] m_debugWeaponDatas = default;

    [SerializeField]
    string m_weaponListFileName = "/WeaponList.json";

    Dictionary<WeaponListTypes, GameObject> m_weaponListDic = new Dictionary<WeaponListTypes, GameObject>();
    Dictionary<WeaponListTypes, WeaponData> m_weaponDataDic = new Dictionary<WeaponListTypes, WeaponData>();
    Dictionary<WeaponListTypes, Image> m_weaponIconsDic = new Dictionary<WeaponListTypes, Image>();

    Transform m_weaponListTrans = default;
    bool m_isChanged = false;
    string dataPath;

    public static WeaponListControl Instance { get; private set; }

    public WeaponData CurrentWeapon1Data { get => m_currentEquipWeapons.Weapon1; }
    public WeaponData CurrentWeapon2Data { get => m_currentEquipWeapons.Weapon2; }
    public WeaponData CurrentWeapon3Data { get => m_currentEquipWeapons.Weapon3; }
    public WeaponData MainWeaponData { get => m_currentEquipWeapons.MainWeapon; }

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        m_weaponListTrans = GameObject.FindGameObjectWithTag("WeaponList").transform;

        if (isDebug)
        {
            m_currentEquipWeapons = new WeaponList(m_debugWeaponDatas[0], m_debugWeaponDatas[1], m_debugWeaponDatas[2], m_debugWeaponDatas[3]);
        }
        else
        {

        }

        Setup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(WeaponListTypes.Equip1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(WeaponListTypes.Equip2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(WeaponListTypes.Equip3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeWeapon(WeaponListTypes.MainWeapon);
        }

    }

    void Setup()
    {
        //武器オブジェクトの登録
        //生成したオブジェクトとアニメーションのオブジェクト名と一致させるために(clone)を削除する処理を行う
        var g1 = m_weaponObjects.FirstOrDefault(o => o.name == m_currentEquipWeapons.Weapon1.WeaponType.ToString());
        m_weaponListDic[WeaponListTypes.Equip1] = Instantiate(g1, m_weaponListTrans);
        m_weaponListDic[WeaponListTypes.Equip1].name = g1.name;

        var g2 = m_weaponObjects.FirstOrDefault(o => o.name == m_currentEquipWeapons.Weapon2.WeaponType.ToString());
        m_weaponListDic[WeaponListTypes.Equip2] = Instantiate(g2, m_weaponListTrans);
        m_weaponListDic[WeaponListTypes.Equip2].name = g2.name;

        var g3 = m_weaponObjects.FirstOrDefault(o => o.name == m_currentEquipWeapons.Weapon3.WeaponType.ToString());
        m_weaponListDic[WeaponListTypes.Equip3] = Instantiate(g3, m_weaponListTrans);
        m_weaponListDic[WeaponListTypes.Equip3].name = g3.name;

        var g4 = m_weaponObjects.FirstOrDefault(o => o.name == m_currentEquipWeapons.MainWeapon.WeaponType.ToString());
        m_weaponListDic[WeaponListTypes.MainWeapon] = Instantiate(g4, m_weaponListTrans);
        m_weaponListDic[WeaponListTypes.MainWeapon].name = g4.name;

        //武器データの登録
        m_weaponDataDic[WeaponListTypes.Equip1] = CurrentWeapon1Data;
        m_weaponDataDic[WeaponListTypes.Equip2] = CurrentWeapon2Data;
        m_weaponDataDic[WeaponListTypes.Equip3] = CurrentWeapon3Data;
        m_weaponDataDic[WeaponListTypes.MainWeapon] = MainWeaponData;

        //武器アイコンの登録
        m_weaponIconsDic[WeaponListTypes.Equip1] = m_weaponIcons[0];
        m_weaponIconsDic[WeaponListTypes.Equip2] = m_weaponIcons[1];
        m_weaponIconsDic[WeaponListTypes.Equip3] = m_weaponIcons[2];
        m_weaponIconsDic[WeaponListTypes.MainWeapon] = m_weaponIcons[3];

        foreach (var w in m_weaponListDic)
        {
            Debug.Log(w.Value.name);
        }

        ChangeWeapon(WeaponListTypes.MainWeapon);
    }

    /// <summary>
    /// 武器を切り替える
    /// </summary>
    /// <param name="type"> 武器リストの種類 </param>
    public void ChangeWeapon(WeaponListTypes type)
    {
        //既に装備中、または一定時間経過していなければ何もしない
        if (m_currentWeapon == type　|| m_isChanged)
        {
            return;
        }

        //現在の装備状況を更新する
        m_currentWeapon = type;

        m_isChanged = true;
        StartCoroutine(ChangeInterval());

        foreach (var w in m_weaponListDic)
        {
            if (w.Key == m_currentWeapon)
            {
                w.Value.SetActive(true);
                m_weaponIconsDic[w.Key].sprite = m_weaponDataDic[w.Key].ActiveWeaponImage;
            }
            else
            {
                w.Value.SetActive(false);
                m_weaponIconsDic[w.Key].sprite = m_weaponDataDic[w.Key].DeactiveWeaponImage;
            }
        }
    }

    /// <summary>
    /// 武器切り替えのインターバル
    /// </summary>
    IEnumerator ChangeInterval()
    {
        float timer = 0;

        while (timer <= m_interval)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        m_isChanged = false;
    }
}
