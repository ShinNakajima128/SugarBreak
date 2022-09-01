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

    [SerializeField]
    Image _lockImage = default;

    Vector3 m_originScale = default;
    Button _button;

    private void Start()
    {
        m_originScale = transform.localScale;
        BaseUI.OnButtonScaleReset += OffSelectButton;
        _buttonText.color = m_unselectColor;
        m_buttonBackground.SetActive(false);
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OffSelectButton);

        if (_lockImage != null)
        {
            if (!_button.interactable)
            {
                _lockImage.enabled = true;
            }
        }    
    }

    public void OnSelectButton()
    {
        transform.DOScale(new Vector3(m_changeScaleValue, m_changeScaleValue, 1), m_animasionSpeed);
        
        //if (m_selectIcon != null && _button.interactable)
        //{
        //    m_selectIcon.enabled = true;
        //    m_unselectIcon.enabled = false;
        //}

        if (m_selectIcon != null)
        {
            m_selectIcon.enabled = true;
            m_unselectIcon.enabled = false;
        }

        //if (_button.interactable)
        //{
        //    m_buttonBackground.SetActive(true);
        //    _buttonText.color = m_selectColor;
        //}

        m_buttonBackground.SetActive(true);
        _buttonText.color = m_selectColor;
    }

    public void OffSelectButton()
    {
        transform.DOScale(m_originScale, m_animasionSpeed);

        //if (m_selectIcon != null && _button.interactable)
        //{
        //    m_selectIcon.enabled = false;
        //    m_unselectIcon.enabled = true;
        //}

        if (m_selectIcon != null)
        {
            m_selectIcon.enabled = false;
            m_unselectIcon.enabled = true;
        }
        
        //if (_button.interactable)
        //{
        //    m_buttonBackground.SetActive(false);
        //    _buttonText.color = m_unselectColor;
        //}

        m_buttonBackground.SetActive(false);
        _buttonText.color = m_unselectColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            OnSelectButton();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            OffSelectButton();
        }
    }
}
