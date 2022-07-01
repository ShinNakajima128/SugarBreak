using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 武器を配置する画面の機能を持つクラス
/// </summary>
public class WeaponsPlacement : MonoBehaviour
{
    [Tooltip("プレイヤーデータ")]
    [SerializeField]
    PlayerData _playerData = default;

    [Tooltip("空の武器データ")]
    [SerializeField]
    WeaponData _emptyData = default;

    [Header("UIObjects")]
    [Tooltip("武器配置画面のObject")]
    [SerializeField]
    GameObject _placementPanel = default;

    [Tooltip("セット先のボタンの各Image")]
    [SerializeField]
    Image[] _placementButtonImages = default;

    [Tooltip("セット先を訊くText")]
    [SerializeField]
    TextMeshProUGUI _askText = default;

    #region private
    WeaponData _currentSelectWeaponData;
    bool _isDirection = false;
    #endregion

    public static Action<WeaponData> OnSetCompleteAction;

    void Start()
    {
        WeaponMenuManager.Instance.OnWeaponButtonClickAction += SetWeaponData;
        WeaponMenuManager.Instance.OnRemoveButtonClickAction += RemoveCurrentWeapon;
        OffPlacementPanel();
    }

    #region button actions
    /// <summary>
    /// 武器をセットする
    /// </summary>
    /// <param name="type"> 押したボタンの種類 </param>
    public void WeaponSet(int type)
    {
        //演出中の場合は処理をしない
        if (_isDirection)
        {
            return;
        }
        _isDirection = true;
        StartCoroutine(SetDirection(type));
    }

    /// <summary>
    /// 配置画面を非表示にする
    /// </summary>
    public void OffPlacementPanel()
    {
        _placementPanel.SetActive(false);
    }
    #endregion

    /// <summary>
    /// 武器セット時の演出
    /// </summary>
    /// <param name="type"> 押したボタンの種類 </param>
    /// <returns></returns>
    IEnumerator SetDirection(int type)
    {
        switch (type)
        {
            case 0:
                _playerData.CurrentWeaponList.Weapon1.IsEquipped = false;
                _playerData.CurrentWeaponList.Weapon1 = _currentSelectWeaponData;
                break;
            case 1:
                _playerData.CurrentWeaponList.Weapon2.IsEquipped = false;
                _playerData.CurrentWeaponList.Weapon2 = _currentSelectWeaponData;
                break;
            case 2:
                _playerData.CurrentWeaponList.Weapon3.IsEquipped = false;
                _playerData.CurrentWeaponList.Weapon3 = _currentSelectWeaponData;
                break;
            case 3:
                //メイン武器の位置を選択した場合は処理を終了する
                _askText.text = "その場所には現在セットできません";

                yield return new WaitForSeconds(1.0f);
                
                _isDirection = false;
                _askText.text = "セット先を選択してください"; //文章を元に戻す
                yield break;
        }
        
        //セットした武器をセット済みにしてImageを更新
        _currentSelectWeaponData.IsEquipped = true;
        _askText.text = "武器をセットしました";
        SetImage();
        OnSetCompleteAction?.Invoke(_currentSelectWeaponData);

        yield return new WaitForSeconds(1.0f);

        _isDirection = false;
        _placementPanel.SetActive(false);
        _askText.text = "セット先を選択してください"; //文章を元に戻す
    }

    /// <summary>
    /// 武器データをセットする
    /// </summary>
    /// <param name="data"> 武器データ </param>
    void SetWeaponData(WeaponData data)
    {
        _currentSelectWeaponData = data;
        _askText.text = "セット先を選択してください";
        SetImage();
    }

    /// <summary>
    /// 各ボタンに現在の装備のImageをセットする
    /// </summary>
    void SetImage()
    {
        _placementButtonImages[0].sprite = _playerData.CurrentWeaponList.Weapon1.ActiveWeaponImage;
        _placementButtonImages[1].sprite = _playerData.CurrentWeaponList.Weapon2.ActiveWeaponImage;
        _placementButtonImages[2].sprite = _playerData.CurrentWeaponList.Weapon3.ActiveWeaponImage;
        _placementButtonImages[3].sprite = _playerData.CurrentWeaponList.MainWeapon.ActiveWeaponImage;
    }

    /// <summary>
    /// 現在選択中の武器をリストから外す
    /// </summary>
    void RemoveCurrentWeapon()
    {
        if (_currentSelectWeaponData == _playerData.CurrentWeaponList.Weapon1) //1つ目にセットされている場合
        {
            _playerData.CurrentWeaponList.Weapon1 = _emptyData;
        }
        else if (_currentSelectWeaponData == _playerData.CurrentWeaponList.Weapon2) //2つ目にセットされている場合
        {
            _playerData.CurrentWeaponList.Weapon2 = _emptyData;
        }
        else if (_currentSelectWeaponData == _playerData.CurrentWeaponList.Weapon3) //3つ目にセットされている場合
        {
            _playerData.CurrentWeaponList.Weapon3 = _emptyData;
        }
        
        Debug.Log($"{_currentSelectWeaponData}を外しました");

        //装備フラグをfalseにする
        _currentSelectWeaponData.IsEquipped = false;
        SetImage();
    }
}
