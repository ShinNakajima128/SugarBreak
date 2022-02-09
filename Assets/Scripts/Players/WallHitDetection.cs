using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁に当たっている時の処理を行うクラス
/// </summary>
public class WallHitDetection : MonoBehaviour
{
    [Header("壁判定用のRayを中央から飛ばす位置")]
    [SerializeField]
    Vector3 m_centerWallRayOffset = new Vector3(0, 0.1f, 0);

    [Header("壁判定用のRayを左から飛ばす位置")]
    [SerializeField]
    Vector3 m_leftWallRayOffset = new Vector3(0, 0.1f, 0);

    [Header("左のRayの角度")]
    [SerializeField]
    Vector3 m_leftRayAngle = new Vector3(-1, 0, 1);

    [Header("壁判定用のRayを右から飛ばす位置")]
    [SerializeField]
    Vector3 m_rightWallRayOffset = new Vector3(0, 0.1f, 0);

    [Header("右のRayの角度")]
    [SerializeField]
    Vector3 m_rightRayAngle = new Vector3(1, 0, 1);

    [Header("昇れる段差")]
    [SerializeField]
    float m_stepOffset = 0.3f;

    [Header("Rayを飛ばす距離")]
    [SerializeField]
    float m_wallDistance = 0.3f;

    [Header("昇れる角度")]
    [SerializeField]
    float m_slopeLimit = 0.3f;

    [Header("昇れる段差の位置から飛ばすレイの距離")]
    [SerializeField]
    float m_slopeDistance = 0.6f;

    Vector3 m_velocity = default;
    Rigidbody m_rb = default;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        var stepRayCenterPosition = transform.position + m_centerWallRayOffset;
        var stepRayLeftPosition = transform.position + m_leftWallRayOffset;
        var stepRayRightPosition = transform.position + m_rightWallRayOffset;
        RaycastHit stepHit;

        if (Physics.Linecast(stepRayCenterPosition, stepRayCenterPosition + transform.forward * m_wallDistance, out stepHit)||
            Physics.Linecast(stepRayLeftPosition, stepRayLeftPosition + m_leftRayAngle * m_wallDistance, out stepHit) ||
            Physics.Linecast(stepRayRightPosition, stepRayRightPosition + m_rightRayAngle * m_wallDistance, out stepHit))
        {
            //　進行方向の地面の角度が指定以下、または昇れる段差より下だった場合の移動処理
            if (Vector3.Angle(transform.transform.up, stepHit.normal) <= m_slopeLimit
            || (Vector3.Angle(transform.transform.up, stepHit.normal) > m_slopeLimit
                && !Physics.Linecast(transform.position + new Vector3(0f, m_stepOffset, 0f), transform.position + new Vector3(0f, m_stepOffset, 0f) + transform.transform.forward * m_slopeDistance))
            )
            {
                PlayerController.Instance.WallHit = false;
            }
            else
            {
                PlayerController.Instance.WallHit = true;
            }
        }
    }

    /// <summary>
    /// 壁との当たり判定用のRayCastを描写する
    /// </summary>
    void OnDrawGizmos()
    {
        //　衝突確認のギズモ
        var stepRayCenterPosition = transform.position + m_centerWallRayOffset;
        var stepRayLeftPosition = transform.position + m_leftWallRayOffset;
        var stepRayRightPosition = transform.position + m_rightWallRayOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayCenterPosition, stepRayCenterPosition + transform.forward * m_wallDistance);
        Gizmos.DrawLine(stepRayLeftPosition, stepRayLeftPosition + m_leftRayAngle * m_wallDistance);
        Gizmos.DrawLine(stepRayRightPosition, stepRayRightPosition + m_rightRayAngle * m_wallDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0f, m_stepOffset, 0f), transform.position + new Vector3(0f, m_stepOffset, 0f) + transform.forward * m_slopeDistance);
    }
}
