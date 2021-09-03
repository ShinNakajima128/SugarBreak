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
            SoundManager.Instance.SwitchBGM("BossBattle");
            Debug.Log("ボスエリアに入った");
            Debug.Log(other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "BakedValley")
            {
                isBattle = false;
                SoundManager.Instance.SwitchBGM("BakedValley");
                Debug.Log("ボスエリアを抜けた");
            }
        }
    }
}
