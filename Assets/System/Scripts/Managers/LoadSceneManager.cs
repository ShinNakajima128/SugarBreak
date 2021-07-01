using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] Fade fade = null;

    void Start()
    {
        fade.FadeOut(2.0f, () =>
        {
            Debug.Log("フェード開始");
        });
    }
}
