using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを再生成する機能を持つクラス
/// </summary>
public class SpawnManager : MonoBehaviour
{
    /// <summary> リスポーンする場所 </summary>
    [SerializeField]
    Transform[] m_spawnPoints = default;

    /// <summary> PlayerのTransform </summary>
    Transform m_playerTrans = default;

    [SerializeField]
    bool m_debugMode = false;

    void Start()
    {
        m_playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!m_debugMode)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            m_playerTrans.position = m_spawnPoints[0].position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            m_playerTrans.position = m_spawnPoints[1].position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            m_playerTrans.position = m_spawnPoints[2].position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            m_playerTrans.position = m_spawnPoints[3].position;
        }
    }
}
