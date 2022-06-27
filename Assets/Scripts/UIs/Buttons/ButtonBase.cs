﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Tooltip("ボタン選択時のScaleの値")]
    [SerializeField]
    float _selectScaleValue = 1.1f;

    [Tooltip("アニメーションの速度")]
    [SerializeField]
    float _animSpeed = 0.25f;

    [Tooltip("カーソルのターゲット")]
    [SerializeField]
    Transform _cursorTarget = default;

    Image _buttonImage;
    Vector3 _originScale;

    void Start()
    {
        _buttonImage = GetComponent<Image>();
        _originScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(_selectScaleValue, _selectScaleValue), _animSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_originScale, _animSpeed);
    }

}
