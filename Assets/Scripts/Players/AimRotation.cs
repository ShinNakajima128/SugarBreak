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

    [Header("スクロールスピード")]
    [SerializeField]
    float m_scrollSpeed = 6.0f;

    [Header("上方向の上限")]
    [SerializeField]
    float m_maxValue = 30;

    [Header("下方向の下限")]
    [SerializeField]
    float m_minValue = -30;

    Transform m_playerTrans = default;
    Transform m_weaponListTrans = default;
    Quaternion m_weaponListOriginTrans;
    float m_currentValue = 0;
    public static AimRotation Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        m_playerTrans = GetComponent<Transform>();
        m_weaponListTrans = GameObject.FindGameObjectWithTag("WeaponList").transform;
        m_weaponListOriginTrans = m_weaponListTrans.transform.rotation;
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
        
        Vector3 playerAngle = new Vector3(0, h * m_rotateSpeed, 0);
        Vector3 weaponListAngle = new Vector3(scroll * m_scrollSpeed, 0, 0);
        m_currentValue += weaponListAngle.x;
        Debug.Log(m_currentValue);
        m_playerTrans.Rotate(playerAngle);

        if (m_currentValue >= m_maxValue)
        {
            m_currentValue = m_maxValue;
            return;
        }
        else if (m_currentValue <= m_minValue)
        {
            m_currentValue = m_minValue;
            return;
        }
        else
        {
            m_weaponListTrans.Rotate(weaponListAngle);
        }
    } 

    /// <summary>
    /// ウェポンリストのRotationを初期化する
    /// </summary>
    public void ResetWeaponListRotation()
    {
        m_weaponListTrans.rotation = m_weaponListOriginTrans;
    }
}
