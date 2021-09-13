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
    SphereCollider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<SphereCollider>();
        isFirst = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isFirst)
            {
                m_collider.radius = 25.5f;
                director.Play();
                isFirst = false;
            }
            SoundManager.Instance.SwitchBGM("BossBattle");
            isBattle = true;
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
                SoundManager.Instance.SwitchBGM("BakedValley");
            }
        }
    }
}
