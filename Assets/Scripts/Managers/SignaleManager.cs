using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class SignaleManager : MonoBehaviour
{
    [Header("フェード用のオブジェクト")]
    [SerializeField] 
    Fade fade = default;

    [SerializeField]
    PostProcessProfile profile = default; 
    [SerializeField]
    MotionBlur volume = default;
    //GameObject m_postManager = default;

    [Header("演出用のボス")]
    [SerializeField] 
    GameObject m_standingDragon = default;

    [Header("戦闘するボス")]
    [SerializeField] GameObject m_mainDragon = default;

    MotionBlur motionBlur;

    private void Awake()
    {
        m_standingDragon.SetActive(false);
        m_mainDragon.SetActive(false);
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
    }

    public void OnMortionBlur()
    {
        //MotionBlur motionBlur = profile.GetSetting<MotionBlur>();
        //motionBlur.enabled.Override(true);
    }

    public void OffMortionBlur()
    {
        //MotionBlur motionBlur = profile.GetSetting<MotionBlur>();
        //motionBlur.enabled.Override(false);
    }
}
