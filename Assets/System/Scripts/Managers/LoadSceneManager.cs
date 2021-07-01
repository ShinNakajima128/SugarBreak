using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] Fade fade = default;
    [SerializeField] SoundManager soundManager = default;

    void Start()
    {
        StartCoroutine(PlaySound());
        fade.FadeOut(1.0f, () =>
        {
            //soundManager.PlaySeByName("Transition");
        });
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.1f);

        soundManager.PlaySeByName("Transition");
    }
}
