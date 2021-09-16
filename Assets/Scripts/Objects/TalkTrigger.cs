﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;

public class TalkTrigger : MonoBehaviour
{
    [Header("フローチャート")]
    [SerializeField] 
    Flowchart m_flowchart = default;

    [Header("呼び出すフローチャートのキーワード")]
    [SerializeField] 
    string m_TalkChart = default;

    [Header("注目する時用のカメラ")]
    [SerializeField] 
    CinemachineFreeLook freeLook = default;

    /// <summary> 表示のフラグ </summary>
    bool isActivated = false;

    [SerializeField]
    bool ChocoEgg = false;

    private void Awake()
    {
        if (m_flowchart == null)
        {
            m_flowchart = GameObject.FindGameObjectWithTag("FlowChart").GetComponent<Flowchart>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.IsBakeleValleyCleared) return;

        ///プレイヤーが来たらフローチャートを再生する
        if (other.gameObject.CompareTag("Player") && !isActivated)
        {
            if (ChocoEgg)
            {
                ActiveCamera();
                StartCoroutine(FinishChart());
            }
            m_flowchart.SendFungusMessage(m_TalkChart);
            isActivated = true;
        }
    }

    IEnumerator FinishChart()
    {
        yield return null;

        while (true)
        {
            if (PlayerStatesManager.Instance.IsOperation)
            {
                InactiveCamera();
                yield break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 注目用のカメラをONにする
    /// </summary>
    public void ActiveCamera()
    {
        freeLook.Priority = 20;
    }
    /// <summary>
    /// 注目用のカメラをOFFにする
    /// </summary>
    public void InactiveCamera()
    {
        freeLook.Priority = 9;
    }

}
