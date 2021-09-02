using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// カメラの距離をマウスホイールで操作するクラス
/// </summary>
public class ZoomController : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook m_freelook = default;
    [SerializeField] float m_minCameraDistance = 6;
    [SerializeField] float m_maxCameraDistance = 11;
    [SerializeField, Range(1F, 5F)] float zoomSpeed = 1;

    void Start()
    {
        m_freelook.m_Orbits[1].m_Radius = m_minCameraDistance;
    }

    void Update()
    {
        if (PlayerStatesManager.Instance.IsOperation)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (m_freelook.m_Orbits[1].m_Radius <= m_maxCameraDistance && scroll < 0)
            {
                m_freelook.m_Orbits[1].m_Radius -= scroll * zoomSpeed;

                if (m_freelook.m_Orbits[1].m_Radius > m_maxCameraDistance)
                {
                    m_freelook.m_Orbits[1].m_Radius = m_maxCameraDistance;
                }
            }
            else if (m_freelook.m_Orbits[1].m_Radius >= 6 && scroll > 0)
            {
                m_freelook.m_Orbits[1].m_Radius -= scroll * zoomSpeed;

                if (m_freelook.m_Orbits[1].m_Radius < m_minCameraDistance)
                {
                    m_freelook.m_Orbits[1].m_Radius = m_minCameraDistance;
                }
            }
        }   
    }
}
