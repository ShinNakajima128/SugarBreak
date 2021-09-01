using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignaleManager : MonoBehaviour
{
    [SerializeField] Fade fade = default;

    public void FadeIn()
    {
        LoadSceneManager.Instance.FadeIn();
    }

    public void FadeOut()
    {
        LoadSceneManager.Instance.FadeOut();
    }
}
