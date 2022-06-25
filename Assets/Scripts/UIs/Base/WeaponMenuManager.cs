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
    [SerializeField]
    PlayerData m_data = default;

    [Tooltip("拠点のUIを管理するクラス")]
    [SerializeField]
    BaseUI m_baseUI = default;

    [Tooltip("金平糖の数を表示するText")]
    [SerializeField]
    TextMeshProUGUI m_sugarPlumText = default;

    /// <summary> 現在の金平糖の数 </summary>
    int _currentSugarPlumNum = 0;

    /// <summary> モデルの回転処理をまとめたAction </summary>
    public Action<RotateType> OnRotateAction = default;

    public static WeaponMenuManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _currentSugarPlumNum = m_data.TotalKonpeitou;
        m_sugarPlumText.text = _currentSugarPlumNum.ToString();
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

    /// <summary>
    /// 装備画面に戻る
    /// </summary>
    public void BackEquipmentPanel()
    {
        m_baseUI.OnEquipment();
    }
}
