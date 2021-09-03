using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignaleManager : MonoBehaviour
{
    [SerializeField] Fade fade = default;
    [SerializeField] GameObject m_standingDragon = default;
    [SerializeField] GameObject m_mainDragon = default;

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
}
