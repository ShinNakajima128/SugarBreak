using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArea : MonoBehaviour
{
    public static bool isBattle = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isBattle = true;
            SoundManager.Instance.PlayBgmByName("BossBattle");
            Debug.Log("ボスエリアに入った");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "BakedValley")
            {
                isBattle = false;
                SoundManager.Instance.PlayBgmByName("BakedValley");
                Debug.Log("ボスエリアを抜けた");
            }
        }
    }
}
