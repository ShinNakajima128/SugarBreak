using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠隔武器のADS時のプレイヤーの回転を管理するクラス
/// </summary>
public class AimRotation : MonoBehaviour
{
    [Header("回転スピード")]
    [SerializeField]
    float m_rotateSpeed = 3.0f;

    [Header("上方向の上限")]
    [SerializeField]
    float m_maxAngle = 5;

    [Header("下方向の下限")]
    [SerializeField]
    float m_minAngle = -5;

    Transform m_playerTrans = default;

    void Start()
    {
        m_playerTrans = GetComponent<Transform>();
    }

    void Update()
    {
        //ADS時以外は処理を行わない
        if (!PlayerController.Instance.IsAimed || !PlayerStatesManager.Instance.IsOperation)
        {
            return;
        }

        float h = Input.GetAxisRaw("Camera X");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        //プレイヤーの上下角度が上限に達していたらそれ以上は向かないようにする
        
        Vector3 angle = new Vector3(scroll * m_rotateSpeed * 2, h * m_rotateSpeed, 0);
        m_playerTrans.Rotate(angle);
    }
}
