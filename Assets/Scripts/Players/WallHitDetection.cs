using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHitDetection : MonoBehaviour
{
    [Header("壁判定用のRayを飛ばす位置")]
    [SerializeField]
    Vector3 m_WallRayOffset = new Vector3(0, 0.1f, 0);

    [Header("昇れる段差")]
    [SerializeField]
    float m_stepOffset = 0.5f;

    [Header("Rayを飛ばす距離")]
    [SerializeField]
    float m_wallDistance = 0.5f;

    [Header("昇れる角度")]
    [SerializeField]
    float m_slopeLimit = 45f;

    [Header("昇れる段差の位置から飛ばすレイの距離")]
    [SerializeField]
    float m_slopeDistance = 0.6f;

    public bool WallDetection()
    {
        var stepRayPosition = transform.position + m_WallRayOffset;

        if (Physics.Linecast(stepRayPosition, stepRayPosition + transform.forward * m_wallDistance, out var stepHit))
        {
            //　進行方向の地面の角度が指定以下、または昇れる段差より下だった場合の移動処理
            if (Vector3.Angle(transform.transform.up, stepHit.normal) <= m_slopeLimit
            || (Vector3.Angle(transform.transform.up, stepHit.normal) > m_slopeLimit
                && !Physics.Linecast(transform.position + new Vector3(0f, m_stepOffset, 0f), transform.position + new Vector3(0f, m_stepOffset, 0f) + transform.forward * m_slopeDistance))
            )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 壁との当たり判定用のRayCastを描写する
    /// </summary>
    void OnDrawGizmos()
    {
        //　衝突確認のギズモ
        var stepRayCenterPosition = transform.position + m_WallRayOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayCenterPosition, stepRayCenterPosition + transform.forward * m_wallDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0f, m_stepOffset, 0f), transform.position + new Vector3(0f, m_stepOffset, 0f) + transform.forward * m_slopeDistance);
    }
}
