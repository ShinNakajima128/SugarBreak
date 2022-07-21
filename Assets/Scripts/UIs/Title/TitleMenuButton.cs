using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public enum ButtonType
{
    /// <summary> はじめから </summary>
    NewGame,
    /// <summary> つづきから </summary>
    Continue,
    /// <summary> クレジット </summary>
    Crefit,
    /// <summary> 探検モード </summary>
    Explore,
    /// <summary> ボス戦モード </summary>
    BossBattle,
    /// <summary> ゲーム終了 </summary>
    GameEnd
}

public class TitleMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [Tooltip("ボタンが押せるかどうか")]
    [SerializeField]
    bool _intaractable = true;

    [Tooltip("ボタンの種類")]
    [SerializeField]
    ButtonType _buttonType = default;

    [Tooltip("アニメーションの速度")]
    [SerializeField]
    float _animSpeed = 0.1f;

    [Tooltip("ボタン選択時の大きさ")]
    [SerializeField]
    float _selectButtonScale = 1.1f;

    [Tooltip("ボタン選択時のアニメーションの移動量")]
    [SerializeField]
    float _selectAnimPosValue = 30f;

    [Tooltip("通常の文字の色")]
    [SerializeField]
    Color _defaultColor = default;

    [Tooltip("選択時の文字の色")]
    [SerializeField]
    Color _enteredColor = default;

    [Tooltip("ボタン選択時のMaskAnimation用のObject")]
    [SerializeField]
    GameObject _animImageObject = default;

    [Tooltip("ボタンのText")]
    [SerializeField]
    TextMeshProUGUI _buttonText = default;

    public Action Enter;
    public Action Click;
    public Action Exit;

    Button _menuButton; 
    Vector3 _originPos;
    Vector3 _enteredPos;

    public Button MenuButton => _menuButton;

    public ButtonType ButtonType => _buttonType;
    
    void Start()
    {
        _originPos = transform.localPosition;
        _enteredPos = transform.localPosition + new Vector3(_selectAnimPosValue, 0, 0);
        _buttonText.color = _defaultColor;
        _animImageObject.SetActive(false);

        _menuButton = GetComponent<Button>();
        _menuButton.interactable = _intaractable;
    }

    void OnEnterAnim()
    {
        transform.DOScale(_selectButtonScale, _animSpeed);
        transform.DOLocalMove(_enteredPos, _animSpeed);
        _buttonText.color = _enteredColor;
        _animImageObject.SetActive(true);
    }

    void OnExitAnim()
    {
        transform.DOScale(Vector3.one, _animSpeed);
        transform.DOLocalMove(_originPos, _animSpeed);
        _buttonText.color = _defaultColor;
        _animImageObject.SetActive(false);
    }

    void StatusReset()
    {
        transform.DOScale(Vector3.one, _animSpeed);
        transform.localPosition = _originPos;
        _buttonText.color = _defaultColor;
        _animImageObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_intaractable)
        {
            Enter?.Invoke();
            OnEnterAnim();
            AudioManager.PlaySE(SEType.UI_CursolMove);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_intaractable)
        {
            Click?.Invoke();
            StatusReset();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_intaractable)
        {
            Exit?.Invoke();
            OnExitAnim();
        }    }
}
