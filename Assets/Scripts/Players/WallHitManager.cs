using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁に当たっている時の処理を行うクラス
/// </summary>
public class WallHitManager : MonoBehaviour
{
    [Header("各方向の壁判定クラス")]
    [SerializeField]
    WallHitDetection[] m_detections = default;

    private void Update()
    {
        //前方に壁があるか調べる
        if (CheckWallHit())
        {
            PlayerController.Instance.WallHit = true;
        }
        else
        {
            PlayerController.Instance.WallHit = false;
        }
    }

    /// <summary>
    /// 壁に振れているか調べる
    /// </summary>
    /// <returns> 当たっているかいないかのフラグ </returns>
    bool CheckWallHit()
    {
        for (int i = 0; i < m_detections.Length; i++)
        {
            if (m_detections[i].WallDetection())
            {
                return true;
            }
        }
        return false;
    }
}
