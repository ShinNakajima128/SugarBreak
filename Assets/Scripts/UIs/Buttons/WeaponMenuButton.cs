using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponMenuButton : ButtonBase
{
    [Tooltip("ボタン内に表示するText")]
    [SerializeField]
    TextMeshProUGUI _buttonText = default;

    Button _menuButton;

    protected override void Start()
    {
        _menuButton = GetComponent<Button>();
    }

    /// <summary>
    /// 装備ボタンを表示する武器によって機能を切り替える
    /// </summary>
    public void OnEquipButton(WeaponData data)
    {
        if (!data.IsUnrocked)
        {
            return;
        }
        StartCoroutine(ButtonSetup(data));
    }

    /// <summary>
    /// ボタンのセットアップを行う
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    IEnumerator ButtonSetup(WeaponData data)
    {
        yield return null; //初アクティブ時にGetConponentの処理を待つ

        //装備済みの場合
        if (data.IsEquipped)
        {
            _menuButton.interactable = false;
            _buttonText.text = "装備済み";
        }
        else
        {
            _menuButton.interactable = true;
            _buttonText.text = "装備する";
        }
    }
}
