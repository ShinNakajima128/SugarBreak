using System;
using UnityEngine;

/// <summary>
/// 武器の種類
/// </summary>
public enum WeaponTypes
{
    None,
    MainWeapon,
    CandyBeat,
    PopLauncher,
    DualSoda
}

[CreateAssetMenu(menuName = "MyScriptable/Create WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("武器の名前")]
    [SerializeField]
    string m_weaponName = "";

    [Header("武器の説明文")]
    [TextArea(1, 10)]
    [SerializeField]
    string m_description = "";

    [Header("武器の種類")]
    [SerializeField]
    WeaponTypes m_weaponType = default;

    [Header("武器を装備しているか")]
    [SerializeField]
    bool m_isEquiped = false;

    [Header("武器がアンロックされているか")]
    [SerializeField]
    bool m_isUnlocked = false;

    [Header("武器のベース素材を獲得しているか")]
    [SerializeField]
    bool m_isGetWeaponmaterial = false;

    [Header("作成に必要な金平糖の数")]
    [SerializeField]
    int m_requireCreateKonpeitoNum = 0;

    [Header("使用している時の画像")]
    [SerializeField]
    Sprite m_activeWeaponImage = default;

    [Header("使用していない時の画像")]
    [SerializeField]
    Sprite m_deactiveWeaponImage = default;

    [Header("未開放時に表示する画像")]
    [SerializeField]
    Sprite m_lockWeaponImage = default;

    [SerializeField]
    GameObject m_WeaponObject = default;

    public Action<bool> OnEquipAction = default;

    public string WeaponName => m_weaponName;

    public string Description => m_description;

    public WeaponTypes WeaponType => m_weaponType;
    public Sprite ActiveWeaponImage => m_activeWeaponImage;

    public Sprite DeactiveWeaponImage => m_deactiveWeaponImage;
    public Sprite LockWeaponImage => m_lockWeaponImage;
    public GameObject WeaponObject => m_WeaponObject;

    public bool IsEquipped
    {
        get => m_isEquiped;
        set
        {
            m_isEquiped = value;
            OnEquipAction?.Invoke(m_isEquiped); //装備しているか否かに応じた処理をセット時に実行
        }
    }
    public bool IsUnrocked { get => m_isUnlocked; set => m_isUnlocked = value; }
    
    /// <summary>
    /// 武器のベース素材を獲得しているか確認する
    /// </summary>
    public bool IsGetWeaponMaterial { get => m_isGetWeaponmaterial; set => m_isGetWeaponmaterial = value; }
    public int GetRequireCreateKonpeitoNum => m_requireCreateKonpeitoNum;

    /// <summary>
    /// 作成可能かどうかを確認する
    /// </summary>
    public bool IsCreating
    {
        get
        {
            return m_requireCreateKonpeitoNum <= DataManager.Instance.GetPlayerData.TotalKonpeitou;
        }
    }
}
