using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class BossArea : MonoBehaviour
{
    [SerializeField] 
    PlayableDirector director = default;

    public static bool isBattle = false;
    public static bool isFirst = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isFirst)
            {
                director.Play();
                isFirst = false;
            }
            else
            {
                SoundManager.Instance.SwitchBGM("BossBattle");

            }
            isBattle = true;
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
