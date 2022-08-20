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
        if (GameManager.Instance.IsPlayingMovie)
        {
            return;
        }

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
                EventManager.OnEvent(Events.BossBattleStart);
            }
            
            
            switch (GameManager.Instance.CurrentStage.StageType)
            {
                case StageTypes.BakeleValley:
                    AudioManager.PlayBGM(BGMType.BakeleValley_Boss);
                    isBattle = true;
                    StartCoroutine(DelayActiveArea());
                    break;
                case StageTypes.RaindyClouds:
                    break;
                case StageTypes.DesertResort:
                    break;
                case StageTypes.GlaseSnowField:
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.Instance.IsPlayingMovie)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player") && PlayerStatesManager.Instance.IsDying)
        {
            switch (GameManager.Instance.CurrentStage.StageType)
            {
                case StageTypes.BakeleValley:
                    AudioManager.PlayBGM(BGMType.BakeleValley_Main);
                    isBattle = false;
                    m_bossAreaEffect.SetActive(false);
                    break;
                case StageTypes.RaindyClouds:
                    break;
                case StageTypes.DesertResort:
                    break;
                case StageTypes.GlaseSnowField:
                    break;
                default:
                    break;
            }       
        }
    }

    IEnumerator DelayActiveArea()
    {
        yield return new WaitForSeconds(1.0f);

        m_bossAreaEffect.SetActive(true);
    }
}
