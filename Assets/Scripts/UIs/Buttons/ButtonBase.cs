using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class ButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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

    Vector3 _originScale;
    protected Action Enter;
    protected Action Click;
    protected Action Exit;

    void Start()
    {
        _originScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit?.Invoke();
    }
}
