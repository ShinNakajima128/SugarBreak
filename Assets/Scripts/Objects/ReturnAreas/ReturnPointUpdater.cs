using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ外に落下した時に復活する場所を更新するクラス
/// </summary>
public class ReturnPointUpdater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ReturnArea.Instance.ReturnPoint = transform.position;
        }
    }
}
