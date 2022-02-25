using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// プレイヤーを策定するクラス
/// </summary>
public class PlayerSearcher : MonoBehaviour
{
    /// <summary> 敵の共通するクラス </summary>
    [SerializeField]
    BossMotionTest m_enemy = default;

    /// <summary> 索敵の角度 </summary>
    [SerializeField]
    float m_searchAngle = 130f;

    /// <summary> 見失うまでの時間 </summary>
    [SerializeField]
    float m_playerLostTime = 5.0f;

    /// <summary> 索敵範囲 </summary>
    [SerializeField]
    SphereCollider m_searchArea = default;

    /// <summary> プレイヤーのTransform </summary>
    Vector3 m_playerPosition = default;

    Quaternion m_playerRotation = default;

    /// <summary> 見つけているか </summary>
    public bool IsFind { get; private set; } = false;

    /// <summary> 索敵の範囲内か </summary>
    public bool IsWithinRange { get; private set; } = false;

    /// <summary> プレイヤーのPosition </summary>
    public Vector3 PlayerPosition => m_playerPosition;
    /// <summary> プレイヤーのRotation </summary>
    public Quaternion PlayerRotation => m_playerRotation;

    void OnTriggerStay(Collider other)
    {
        if (m_enemy.CurrentState == EnemyState.dead) return;

        if (other.gameObject.tag == "Player")
        {
            IsWithinRange = true;
            //　主人公の方向
            var playerDirection = other.transform.position - transform.position;
            //　敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったら発見
            if (IsWithinRange)
            {
                m_playerPosition = other.transform.position;
                if (angle <= m_searchAngle)
                {
                    IsFind = true;
                }
                else
                {
                    IsFind = false;
                }
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        IsWithinRange = false;
        StartCoroutine(LostTimer());
    }

    /// <summary>
    /// プレイヤーを見失うまでのタイマー
    /// </summary>
    IEnumerator LostTimer()
    {
        float timer = 0;

        while (timer < m_playerLostTime)
        {
            if (IsWithinRange)
            {
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        IsFind = false;
    }

#if UNITY_EDITOR
    //　サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = new Color(1, 0, 0, 0.3f);
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -m_searchAngle, 0f) * transform.forward, m_searchAngle * 2f, m_searchArea.radius);
    }
#endif
}
