using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;

public class TalkTrigger : MonoBehaviour
{
    /// <summary> フローチャート </summary>
    [SerializeField] Flowchart m_flowchart = default;
    /// <summary> 呼び出すフローチャートのキーワード </summary>
    [SerializeField] string m_TalkChart = default;
    /// <summary> 注目する時用のカメラ </summary>
    [SerializeField] CinemachineFreeLook freeLook = default;
    /// <summary> 表示のフラグ </summary>
    bool isActivated = false;

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
        freeLook.Priority = 12;
    }
    /// <summary>
    /// 注目用のカメラをOFFにする
    /// </summary>
    public void InactiveCamera()
    {
        freeLook.Priority = 9;
    }

}
