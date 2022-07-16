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
    bool _isActivated = false;

    [SerializeField]
    bool _chocoEgg = false;

    PlayerData _playerData;

    private void Awake()
    {
        if (m_flowchart == null)
        {
            m_flowchart = GameObject.FindGameObjectWithTag("FlowChart").GetComponent<Flowchart>();
        }
        _playerData = DataManager.Instance.GetPlayerData;
    }

    private void OnTriggerEnter(Collider other)
    {
        //現在のステージをクリアしていたらチュートリアルを表示しない
        if (GameManager.Instance.CurrentStage.IsStageCleared) 
        { 
            return; 
        }

        ///プレイヤーが来たらフローチャートを再生する
        if (other.gameObject.CompareTag("Player") && !_isActivated)
        {
            if (_chocoEgg)
            {
                ActiveCamera();
                StartCoroutine(FinishChart());
            }
            m_flowchart.SendFungusMessage(m_TalkChart);
            _isActivated = true;
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
        GameManager.Instance.IsPlayingMovie = true;
    }
    /// <summary>
    /// 注目用のカメラをOFFにする
    /// </summary>
    public void InactiveCamera()
    {
        freeLook.Priority = 9;
        GameManager.Instance.IsPlayingMovie = false;
    }

}
