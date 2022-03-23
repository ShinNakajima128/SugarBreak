using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum OperationUIState
{
    Hidden,
    Keyboard,
    Gamepad
}

/// <summary>
/// 操作方法の表示、非表示を管理するクラス
/// </summary>
public class OperationUIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_guideText = default;

    [SerializeField]
    TextMeshProUGUI m_currentOperationText = default;

    [SerializeField]
    TextMeshProUGUI[] m_OperationTexts = default;

    OperationUIState m_currentState = OperationUIState.Hidden;
    bool m_isInputed = false;

    void Start()
    {
    }

    void Update()
    {
        if (!PlayerStatesManager.Instance.IsOperation && m_currentState == OperationUIState.Hidden)
        {
            return;
        }
        else
        {
            // 方向の入力を取得し、方向を求める
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            // 入力方向のベクトルを組み立てる
            Vector3 dir = Vector3.forward * v + Vector3.right * h;
            if (dir != Vector3.zero)
            {

            }
        }

    }

    void OperationDescriptionChange(OperationUIState state)
    {
        m_currentState = state;

        switch (m_currentState)
        {
            case OperationUIState.Hidden:
                m_OperationTexts[0].enabled = false;
                m_OperationTexts[1].enabled = false;
                break;
            case OperationUIState.Keyboard:
                m_OperationTexts[0].enabled = true;
                m_OperationTexts[1].enabled = false;
                OnAnimation(m_OperationTexts[0]);
                break;
            case OperationUIState.Gamepad:
                m_OperationTexts[0].enabled = false;
                m_OperationTexts[1].enabled = true;
                OnAnimation(m_OperationTexts[1]);
                break;
            default:
                break;
        }
    }

    void OnAnimation(TextMeshProUGUI text)
    {
        var originPos = text.rectTransform.position;
        text.rectTransform.position = new Vector3(originPos.x + 100, originPos.y, originPos.z);
        text.rectTransform.DOMoveX(originPos.x, 0.25f);
    }
}
