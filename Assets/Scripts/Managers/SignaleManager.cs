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
    [SerializeField] 
    GameObject m_mainDragon = default;

    [SerializeField]
    GameObject m_weapons = default;

    [SerializeField]
    int m_bossZoomBlueValue = 80;

    [SerializeField]
    int m_clearBlurValue = 50;

    [SerializeField]
    float m_blurSpeed = 0.8f;

    ZoomBlur zoomBlur;
    Coroutine coroutine;

    private void Awake()
    {
        if(m_standingDragon) m_standingDragon.SetActive(false);
        m_volume.profile.TryGet(out zoomBlur);
    }

    public void FadeIn()
    {
        LoadSceneManager.Instance.FadeIn(LoadSceneManager.Instance.Masks[2]);
    }

    public void FadeOut()
    {
        LoadSceneManager.Instance.FadeOut(LoadSceneManager.Instance.Masks[1]);
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
        coroutine = StartCoroutine(IncreaseParameter(m_bossZoomBlueValue));
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
    }

    public void PlayClearBGM()
    {
        SoundManager.Instance.PlayBgmByName("ClearJingle");
    }

    public void OnClearBlur()
    {
        coroutine = StartCoroutine(IncreaseParameter(m_clearBlurValue));
    }

    public void OffClearBlur()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(DecreaseParameter());
    }

    public void OffWeapons()
    {
        m_weapons?.SetActive(false);
    }

    IEnumerator IncreaseParameter(float value)
    {
        while (zoomBlur.focusPower.value < value)
        {
            zoomBlur.focusPower.value += m_blurSpeed;
            yield return null;
        }
    }

    IEnumerator DecreaseParameter()
    {
        while (zoomBlur.focusPower.value > 0)
        {
            zoomBlur.focusPower.value -= m_blurSpeed;
            yield return null;
        }
    }
}
