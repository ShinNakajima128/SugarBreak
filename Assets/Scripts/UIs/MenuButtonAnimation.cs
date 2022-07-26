using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField]
    Image m_selectIcon = default;

    [SerializeField]
    Image m_unselectIcon = default;

    [SerializeField]
    GameObject m_buttonBackground = default;

    [SerializeField]
    Text _buttonText = default;

    [SerializeField]
    Color m_selectColor = default;

    [SerializeField]
    Color m_unselectColor = default;

    Vector3 m_originScale = default;

    private void Start()
    {
        m_originScale = transform.localScale;
        BaseUI.OnButtonScaleReset += OffSelectButton;
        _buttonText.color = m_unselectColor;
        m_buttonBackground.SetActive(false);
        var button = GetComponent<Button>();

        button.onClick.AddListener(OffSelectButton);
    }

    public void OnSelectButton()
    {
        transform.DOScale(new Vector3(m_changeScaleValue, m_changeScaleValue, 1), m_animasionSpeed);
        
        if (m_selectIcon != null)
        {
            m_selectIcon.enabled = true;
            m_unselectIcon.enabled = false;
        }

        m_buttonBackground.SetActive(true);
        _buttonText.color = m_selectColor;
    }

    public void OffSelectButton()
    {
        transform.DOScale(m_originScale, m_animasionSpeed);
        
        if (m_selectIcon != null)
        {
            m_selectIcon.enabled = false;
            m_unselectIcon.enabled = true;
        }

        m_buttonBackground.SetActive(false);
        _buttonText.color = m_unselectColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelectButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OffSelectButton();
    }
}
