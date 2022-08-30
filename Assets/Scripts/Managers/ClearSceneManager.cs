using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearSceneManager : MonoBehaviour
{
    [SerializeField]
    Fade _fade = default;

    [SerializeField]
    Image _charaImage = default;

    [SerializeField]
    Sprite _winkImage = default;

    Coroutine _fadeCol;

    void Start()
    {
        StartCoroutine(ClearDirection());
    }

    IEnumerator ClearDirection()
    {
        yield return new WaitForSeconds(2.0f);

        yield return new WaitUntil(() => Input.anyKeyDown);

        _fadeCol = _fade.FadeIn(3.0f);
        yield return new WaitForSeconds(0.8f);
        
        StopCoroutine(_fadeCol);
        _fadeCol = null;

        _charaImage.sprite = _winkImage;

        yield return new WaitForSeconds(1.5f);

        _fadeCol = _fade.FadeIn(0.5f, () => 
        {
            LoadSceneManager.Instance.LoadTitle();
        });
    }
}
