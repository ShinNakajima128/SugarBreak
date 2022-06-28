using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器リストのボタン
/// </summary>
public class WeaponListButton : ButtonBase
{
    WeaponData _weaponData;

    public WeaponData WeaponButtonData => _weaponData;

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// ボタン生成時にデータをセットする
    /// </summary>
    /// <param name="data"></param>
    public void SetData(WeaponData data)
    {
        _weaponData = data;
        
        //武器が解放済みの場合
        if (data.IsUnrocked)
        {
            _buttonImage.sprite = _weaponData.ActiveWeaponImage;
        }
        else
        {
            _buttonImage.sprite = _weaponData.DeactiveWeaponImage;
        }
    }
}
