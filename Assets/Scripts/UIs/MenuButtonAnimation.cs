using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MenuButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ボタン選択時のScaleの値")]
    [SerializeField]
    float m_changeScaleValue = 1.1f;

    [Header("アニメーションの速度")]
    [SerializeField]
    float m_animasionSpeed = 0.25f;

    Vector3 m_originScale = default;

    private void Start()
    {
        m_originScale = transform.localScale;
        BaseUI.OnButtonScaleReset += OffSelectButton;
    }

    public void OnSelectButton()
    {
        transform.DOScale(new Vector3(m_changeScaleValue, m_changeScaleValue, 1), m_animasionSpeed);
    }

    public void OffSelectButton()
    {
        transform.DOScale(m_originScale, m_animasionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(m_changeScaleValue, m_changeScaleValue, 1), m_animasionSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(m_originScale, m_animasionSpeed);
    }
}
