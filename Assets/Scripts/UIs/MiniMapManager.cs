using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField]
    float m_cameraRange = 20;
    [SerializeField]
    Transform m_miniMapCamera = default;

    Transform _playerTrans;

    void Start()
    {
        _playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        //m_miniMapCamera.SetParent(_playerTrans);
        //m_miniMapCamera.localPosition = new Vector3(0, 20, 0);
    }

    void FixedUpdate()
    {
        m_miniMapCamera.position = new Vector3(_playerTrans.position.x,  m_cameraRange, _playerTrans.position.z); 
    }
}
