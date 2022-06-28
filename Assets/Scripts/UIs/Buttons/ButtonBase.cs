using System;
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
        
        //Enter += (() => 
        //{
        //    MenuCursor.CursorMove(CursorTarget.position);
        //});
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter?.Invoke();
        transform.DOScale(new Vector3(_selectScaleValue, _selectScaleValue, 1), _animSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit?.Invoke();
        transform.DOScale(_originScale, _animSpeed);
    }
}
