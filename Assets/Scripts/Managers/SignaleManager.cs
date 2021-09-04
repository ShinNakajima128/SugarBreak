using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SignaleManager : MonoBehaviour
{
    [Header("フェード用のオブジェクト")]
    [SerializeField] 
    Fade fade = default;

    [SerializeField]
    Volume m_volume = default; 

    [Header("演出用のボス")]
    [SerializeField] 
    GameObject m_standingDragon = default;

    [Header("戦闘するボス")]
    [SerializeField] GameObject m_mainDragon = default;

    ZoomBlur zoomBlur;
    Coroutine coroutine;

    private void Awake()
    {
        if(m_standingDragon) m_standingDragon.SetActive(false);
        m_volume.profile.TryGet<ZoomBlur>(out zoomBlur);
        Debug.Log(zoomBlur);
    }

    public void FadeIn()
    {
        LoadSceneManager.Instance.FadeIn();
    }

    public void FadeOut()
    {
        LoadSceneManager.Instance.FadeOut();
    }

    public void OnDragon()
    {
        m_standingDragon.SetActive(true);
    }

    public void SwitchDragon()
    {
        m_standingDragon.SetActive(false);
        m_mainDragon.SetActive(true);
    }

    public void OnLandingEffect()
    {
        EffectManager.PlayEffect(EffectType.Landing, m_standingDragon.transform.position);
        SoundManager.Instance.PlaySeByName("DragonFrap");
    }

    public void SwitchBossBgm()
    {
        SoundManager.Instance.SwitchBGM("BossBattle");
    }

    public void OnZoomBlur()
    {
        SoundManager.Instance.PlaySeByName("DragonRoar");
        coroutine = StartCoroutine(IncreaseParameter());
        Debug.Log("ブラーオン");
    }

    public void OffZoomBlur()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        zoomBlur.focusPower.value = 0;
        Debug.Log("ブラーオフ");
    }

    IEnumerator IncreaseParameter()
    {
        while (zoomBlur.focusPower.value < 80)
        {
            zoomBlur.focusPower.value += 0.8f;
            yield return null;
        }
    }
}
