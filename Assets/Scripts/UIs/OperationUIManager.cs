using System;
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

    OperationUIState m_currentState = OperationUIState.Keyboard;
    bool m_isOperated = true;
    bool m_isDisplayed = false;
    Vector3 m_originPos = default;

    void Start()
    {
        m_originPos = m_OperationTexts[0].transform.localPosition;
        GuideTextChange();
        OperationDescriptionChange(OperationUIState.Hidden);
    }

    void Update()
    {
        //ムービー中は非表示にする
        if (GameManager.Instance.IsPlayingMovie)
        {
            //切り替わった時に一度だけ処理を行う
            if (m_isOperated)
            {
                OperationDescriptionChange(OperationUIState.Hidden);
                m_guideText.enabled = false;
                m_isOperated = false;
            }
            return;
        }
        else
        {
            //切り替わった時に一度だけ処理を行う
            if (!m_isOperated)
            {
                m_isOperated = true;
                m_guideText.enabled = true;

                //操作方法が表示状態だった場合は表示する
                if (m_isDisplayed)
                {
                    OperationDescriptionChange(m_currentState);
                }
            }

            //操作方法が非表示の場合
            if (!m_isDisplayed)
            {
                if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown("joystick button 9"))
                {
                    m_isDisplayed = true;
                    OperationDescriptionChange(m_currentState);
                    GuideTextChange();
                    return;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown("joystick button 9"))
                {
                    OperationDescriptionChange(OperationUIState.Hidden);
                    m_isDisplayed = false;
                    GuideTextChange();
                    return;
                }

                if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown("joystick button 5"))
                {
                    switch (m_currentState)
                    {
                        case OperationUIState.Keyboard:
                            OperationDescriptionChange(OperationUIState.Gamepad);
                            break;
                        case OperationUIState.Gamepad:
                            OperationDescriptionChange(OperationUIState.Keyboard);
                            break;
                        default:
                            Debug.LogError("不正な値です");
                            break;
                    }
                    Debug.Log("表示切替");
                }
            }
        }
    }

    /// <summary>
    /// 操作説明のTextの状態を切り替える
    /// </summary>
    /// <param name="state"> Textの状態 </param>
    void OperationDescriptionChange(OperationUIState state)
    {
        switch (state)
        {
            case OperationUIState.Hidden:
                if (m_OperationTexts[0].enabled)
                {
                    OffAnimation(m_OperationTexts[0]);
                }
                else
                {
                    OffAnimation(m_OperationTexts[1]);
                }
                m_currentOperationText.text = "";
                break;
            case OperationUIState.Keyboard:
                m_OperationTexts[1].enabled = false;
                OnAnimation(m_OperationTexts[0]);
                m_currentOperationText.text = "【キーボードマウス】";
                m_currentState = OperationUIState.Keyboard;
                Debug.Log("キーボード表示");
                break;
            case OperationUIState.Gamepad:
                m_OperationTexts[0].enabled = false;
                OnAnimation(m_OperationTexts[1]);
                m_currentOperationText.text = "【ゲームパッド】";
                m_currentState = OperationUIState.Gamepad;
                Debug.Log("ゲームパッド表示");
                break;
        }
        GuideTextChange();
    }

    /// <summary>
    /// 表示にするアニメーション
    /// </summary>
    /// <param name="text"> 表示にするText </param>
    void OnAnimation(TextMeshProUGUI text)
    {
        if (!text.enabled)
        {
            text.enabled = true;
        }

        text.rectTransform.localPosition = new Vector3(1214, m_originPos.y, m_originPos.z);
        text.rectTransform.DOLocalMoveX(m_originPos.x, 0.1f);
    }

    /// <summary>
    /// 非表示にするアニメーション
    /// </summary>
    /// <param name="text"> 非表示にするText </param>
    void OffAnimation(TextMeshProUGUI text)
    {
        Debug.Log("OFF呼ばれた");
        if (!text.enabled)
        {
            return;
        }

        text.rectTransform.DOLocalMoveX(1214, 0.1f)
            .OnComplete(() => 
            {
                text.enabled = false;
            });
    }

    /// <summary>
    /// 操作方法の表示方法のTextを変更する
    /// </summary>
    void GuideTextChange()
    {
        //表示状態の場合
        if (m_isDisplayed)
        {
            switch (m_currentState)
            {
                case OperationUIState.Keyboard:
                    m_guideText.text = "V:切り替え B:非表示";
                    break;
                case OperationUIState.Gamepad:
                    m_guideText.text = "Rボタン:切り替え RS押し込み:非表示";
                    break;
            }
        }
        else
        {
            switch (m_currentState)
            {
                case OperationUIState.Keyboard:
                    m_guideText.text = "B:操作方法表示";

                    break;
                case OperationUIState.Gamepad:
                    m_guideText.text = "RS押し込み:操作方法表示";
                    break;
            }
        }
    }
}
