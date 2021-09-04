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
