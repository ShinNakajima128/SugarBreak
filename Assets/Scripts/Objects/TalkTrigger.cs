using System.Collections;
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

    private void Awake()
    {
        if (m_flowchart == null)
        {
            m_flowchart = GameObject.FindGameObjectWithTag("FlowChart").GetComponent<Flowchart>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ///プレイヤーが来たらフローチャートを再生する
        if (other.gameObject.CompareTag("Player") && !isActivated)
        {
            m_flowchart.SendFungusMessage(m_TalkChart);
            isActivated = true;
        }
    }

    /// <summary>
    /// 注目用のカメラをONにする
    /// </summary>
    public void ActiveCamera()
    {
        freeLook.Priority = 16;
    }
    /// <summary>
    /// 注目用のカメラをOFFにする
    /// </summary>
    public void InactiveCamera()
    {
        freeLook.Priority = 9;
    }

}
