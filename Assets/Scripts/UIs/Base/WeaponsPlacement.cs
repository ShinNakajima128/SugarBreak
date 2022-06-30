using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器を配置する画面の機能を持つクラス
/// </summary>
public class WeaponsPlacement : MonoBehaviour
{
    [Tooltip("プレイヤーデータ")]
    [SerializeField]
    PlayerData _playerData = default;

    void Start()
    {
        
    }

    #region button actions
    public void WeaponSet()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
