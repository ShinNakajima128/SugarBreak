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
    [SerializeField]
    float m_interval = 1.0f;

    [SerializeField]
    WeaponList m_currentEquipWeapons = default;

    [SerializeField]
    WeaponListTypes m_currentWeapon = default;

    [SerializeField]
    Image[] m_weaponIcons = default;

    [SerializeField]
    bool isDebug = false;

    [SerializeField]
    WeaponData[] weapons = default;

    [SerializeField]
    string m_weaponListFileName = "/WeaponList.json";

    Dictionary<WeaponListTypes, GameObject> m_weaponListDic = new Dictionary<WeaponListTypes, GameObject>();
    Dictionary<WeaponListTypes, WeaponData> m_weaponDataDic = new Dictionary<WeaponListTypes, WeaponData>();
    Dictionary<WeaponListTypes, Image> m_weaponIconsDic = new Dictionary<WeaponListTypes, Image>();

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
        if (isDebug)
        {
            m_currentEquipWeapons = new WeaponList(weapons[0], weapons[1], weapons[2], weapons[3]);
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
        m_weaponListDic[WeaponListTypes.Equip1] = GameObject.Find(m_currentEquipWeapons.Weapon1.WeaponType.ToString());
        m_weaponListDic[WeaponListTypes.Equip2] = GameObject.Find(m_currentEquipWeapons.Weapon2.WeaponType.ToString());
        m_weaponListDic[WeaponListTypes.Equip3] = GameObject.Find(m_currentEquipWeapons.Weapon3.WeaponType.ToString());
        m_weaponListDic[WeaponListTypes.MainWeapon] = GameObject.Find(m_currentEquipWeapons.MainWeapon.WeaponType.ToString());

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
