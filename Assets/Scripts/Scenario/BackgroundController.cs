using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 背景の切り替えを管理するクラス
/// </summary>
public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    Image[] m_backGroundImages = default;

    [SerializeField]
    Sprite[] m_backGrounds = default;

    [SerializeField]
    float m_switchTime = 1.0f;

    int _backgroundIndex = 0;
    public static BackgroundController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BackgroundSetup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnNextBackground();
        }
    }

    void BackgroundSetup()
    {
        m_backGroundImages[0].enabled = true;
        m_backGroundImages[1].enabled = true;
        m_backGroundImages[0].sprite = m_backGrounds[0];
        m_backGroundImages[1].DOFade(0, 0);
        _backgroundIndex++;
        Debug.Log("set");
    }

    public void OnNextBackground(Action callback = null)
    {
        if (_backgroundIndex > m_backGrounds.Length - 1)
        {
            Debug.LogError("背景の数が足りません");
            return;
        }
        m_backGroundImages[1].sprite = m_backGroundImages[0].sprite;
        m_backGroundImages[0].sprite = m_backGrounds[_backgroundIndex];
        m_backGroundImages[1].DOFade(1, 0);
        m_backGroundImages[1].DOFade(0, m_switchTime)
                             .OnComplete(() =>
                             {
                                 _backgroundIndex++;
                                 callback?.Invoke();
                             });
    }
}
