using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField] float m_forwardSpeed = 3;
    CinemachineFreeLook m_virtualCamera;
    CinemachineOrbitalTransposer m_orbitalTransposer;

    private void Start()
    {
        m_virtualCamera = GetComponent<CinemachineFreeLook>();
        m_orbitalTransposer = m_virtualCamera.GetComponentInChildren<CinemachineOrbitalTransposer>();
    }

    void Update()
    {
        forwardViewPoint();       
    }

    private void forwardViewPoint()
    {
        // マウスホイールの回転値を変数 scroll に渡す
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 offset = m_virtualCamera.transform.forward * scroll * m_forwardSpeed;
         if (m_orbitalTransposer) m_orbitalTransposer.m_FollowOffset -= offset;
        //Debug.Log(offset.ToString());
    }
}
