using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class BossArea : MonoBehaviour
{
    [SerializeField] 
    PlayableDirector director = default;

    [SerializeField]
    GameObject m_bossAreaEffect = default;

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
            else
            {
                //if (!UIManager.Instance.BossUI.activeSelf) UIManager.Instance.BossUI.SetActive(true);
                EventManager.OnEvent(Events.BossBattleStart);
            }
            
            SoundManager.Instance.SwitchBGM("BossBattle");
            isBattle = true;
            m_bossAreaEffect?.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "BakedValley")
            {
                SoundManager.Instance.SwitchBGM("BakeleValley");
                isBattle = false;
                m_bossAreaEffect?.SetActive(false);
            }
        }
    }
}
