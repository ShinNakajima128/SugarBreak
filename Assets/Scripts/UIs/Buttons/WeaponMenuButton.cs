using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MenuButtonType
{
    None,
    Equip,
    Enhance
}

public class WeaponMenuButton : ButtonBase
{
    [SerializeField]
    MenuButtonType _buttonType = default;

    [Tooltip("ボタン内に表示するText")]
    [SerializeField]
    TextMeshProUGUI _buttonText = default;

    Button _menuButton;

    void OnEnable()
    {
        //ボタンが装備ボタンの場合
        if (_buttonType == MenuButtonType.Equip)
        {
            WeaponsPlacement.OnSetCompleteAction += OnEquipButton;
        }
    }

    void OnDisable()
    {
        //ボタンが装備ボタンの場合
        if (_buttonType == MenuButtonType.Equip)
        {
            WeaponsPlacement.OnSetCompleteAction -= OnEquipButton;
        }
    }

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
            _buttonText.text = "外す";
            _menuButton.onClick.RemoveAllListeners();
            _menuButton.onClick.AddListener(() => 
            {
                WeaponMenuManager.Instance.Remove();
                StartCoroutine(ButtonSetup(data));
            });
        }
        else
        {
            _buttonText.text = "装備する";
            _menuButton.onClick.RemoveAllListeners();
            _menuButton.onClick.AddListener(WeaponMenuManager.Instance.Equip);
        }
    }
}
