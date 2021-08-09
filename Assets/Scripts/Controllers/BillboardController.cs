using UnityEngine;

/// <summary>
/// UI オブジェクトをカメラの方向に向ける
/// </summary>
public class BillboardController : MonoBehaviour
{
    void Update()
    {
        // オブジェクトとカメラの方向を合わせる
        this.transform.forward = Camera.main.transform.forward;
    }
}