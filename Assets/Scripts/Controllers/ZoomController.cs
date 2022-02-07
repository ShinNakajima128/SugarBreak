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
    [SerializeField] float m_minOrbitsDistance = 6;
    [SerializeField] float m_maxOrbitsDistance = 11;
    [SerializeField] float m_minFovDistance = 40;
    [SerializeField] float m_maxFovDistance = 60;
    [SerializeField, Range(1F, 5F)] float zoomSpeed = 1;
    [SerializeField] bool m_FovChangeMode = false;

    void Start()
    {
        if (m_FovChangeMode)
        {
            m_freelook.m_Lens.FieldOfView = m_minFovDistance;
        }
        else
        {
            m_freelook.m_Orbits[1].m_Radius = m_minOrbitsDistance;
        }
    }

    void Update()
    {
        if (PlayerStatesManager.Instance.IsOperation && !PlayerController.Instance.IsAimed)
        {
            if (m_FovChangeMode)
            {
                FovChange();
            }
            else
            {
                OrbisChange();
            }
        }   
    }

    void OrbisChange()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (m_freelook.m_Orbits[1].m_Radius < m_maxOrbitsDistance && scroll < 0)
        {
            if (m_freelook.m_Orbits[1].m_Radius > m_maxOrbitsDistance)
            {
                m_freelook.m_Orbits[1].m_Radius = m_maxOrbitsDistance;
                return;
            }
            else
            {
                m_freelook.m_Orbits[0].m_Height -= scroll * zoomSpeed * 0.5f;
                m_freelook.m_Orbits[0].m_Radius -= scroll * zoomSpeed * 0.8f;
                m_freelook.m_Orbits[1].m_Radius -= scroll * zoomSpeed;
                m_freelook.m_Orbits[2].m_Radius -= scroll * zoomSpeed;
            }
        }
        else if (m_freelook.m_Orbits[1].m_Radius > m_minOrbitsDistance && scroll > 0)
        {
            if (m_freelook.m_Orbits[1].m_Radius < m_minOrbitsDistance)
            {
                m_freelook.m_Orbits[1].m_Radius = m_minOrbitsDistance;
                return;
            }
            else
            {
                m_freelook.m_Orbits[0].m_Height -= scroll * zoomSpeed * 0.5f;
                m_freelook.m_Orbits[0].m_Radius -= scroll * zoomSpeed * 0.8f;
                m_freelook.m_Orbits[1].m_Radius -= scroll * zoomSpeed;
                m_freelook.m_Orbits[2].m_Radius -= scroll * zoomSpeed;
            }
        }
    }
    void FovChange()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (m_freelook.m_Lens.FieldOfView < m_maxFovDistance && scroll < 0)
        {
            if (m_freelook.m_Lens.FieldOfView > m_maxFovDistance)
            {
                m_freelook.m_Lens.FieldOfView = m_maxFovDistance;
                return;
            }
            else
            {
                m_freelook.m_Lens.FieldOfView -= scroll * zoomSpeed * 3;
                
            }
        }
        else if (m_freelook.m_Lens.FieldOfView > m_minFovDistance && scroll > 0)
        {
            if (m_freelook.m_Lens.FieldOfView < m_minFovDistance)
            {
                m_freelook.m_Lens.FieldOfView = m_minFovDistance;
                return;
            }
            else
            {
                m_freelook.m_Lens.FieldOfView -= scroll * zoomSpeed * 3;
            }
        }
    }
}
