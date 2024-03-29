﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using SugarBreak;

public abstract class ButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("ボタン選択時のScaleの値")]
    [SerializeField]
    float _selectScaleValue = 1.1f;

    [Tooltip("アニメーションの速度")]
    [SerializeField]
    float _animSpeed = 0.1f;

    [Tooltip("カーソルのターゲット")]
    [SerializeField]
    Transform _cursorTarget = default;

    [Tooltip("ボタンがアニメーションするか")]
    [SerializeField]
    bool _isAnimation = true;

    protected Image _buttonImage;
    Vector3 _originScale;
    public Action Enter;
    public Action Click;
    public Action Exit;

    public Transform CursorTarget => _cursorTarget;

    protected virtual void Start()
    {
        _buttonImage = GetComponent<Image>();
        _originScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click?.Invoke();
        AudioManager.PlaySE(SEType.UI_ButtonSelect);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter?.Invoke();
        OnSelectButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit?.Invoke();
        OnDeselectButton();
    }

    public void OnSelectButton()
    {
        if (_isAnimation)
        {
            transform.DOScale(new Vector3(_selectScaleValue, _selectScaleValue, 1), _animSpeed);
        }
        AudioManager.PlaySE(SEType.UI_CursolMove);
        MenuCursor.CursorMove(CursorTarget.position);
    }

    public void OnDeselectButton()
    {
        if (_isAnimation)
        {
            transform.DOScale(_originScale, _animSpeed);
        }
    }

    public void OnClickButton()
    {
        Click?.Invoke();
        AudioManager.PlaySE(SEType.UI_ButtonSelect);
    }
}
