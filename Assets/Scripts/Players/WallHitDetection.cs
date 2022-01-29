using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁に当たっている時の処理を行うクラス
/// </summary>
public class WallHitDetection : MonoBehaviour
{
    [Header("壁判定用のRayを飛ばす位置")]
    [SerializeField]
    Vector3 m_wallRayOffset = new Vector3(0, 0.1f, 0);

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
    float m_runSpeed = default;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_runSpeed = PlayerController.Instance.RunSpeed;
    }

    private void Update()
    {
        m_velocity = m_rb.velocity;
        var stepRayPosition = m_rb.position + m_wallRayOffset;

        if (Physics.Linecast(stepRayPosition, stepRayPosition + m_rb.transform.forward * m_wallDistance, out var stepHit))
        {
            //　進行方向の地面の角度が指定以下、または昇れる段差より下だった場合の移動処理
            if (Vector3.Angle(m_rb.transform.up, stepHit.normal) <= m_slopeLimit
            || (Vector3.Angle(m_rb.transform.up, stepHit.normal) > m_slopeLimit
                && !Physics.Linecast(m_rb.position + new Vector3(0f, m_stepOffset, 0f), m_rb.position + new Vector3(0f, m_stepOffset, 0f) + m_rb.transform.forward * m_slopeDistance))
            )
            {
                Debug.Log("壁から離れた");
                PlayerController.Instance.WallHit = false;
            }
            else
            {
                PlayerController.Instance.WallHit = true;
            }
        }
        m_rb.velocity = m_velocity;
    }
    void OnDrawGizmos()
    {
        //　衝突確認のギズモ
        var stepRayPosition = transform.position + m_wallRayOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayPosition, stepRayPosition + transform.forward * m_wallDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0f, m_stepOffset, 0f), transform.position + new Vector3(0f, m_stepOffset, 0f) + transform.forward * m_slopeDistance);
    }
}
