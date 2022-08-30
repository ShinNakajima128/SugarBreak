using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearSceneManager : MonoBehaviour
{
    [SerializeField]
    Fade _fade = default;

    [Header("AnimationSpeed")]
    [SerializeField]
    float _circleAnimSpeed = 0.2f;

    [SerializeField]
    float _charaAnimSpeed = 1.0f;

    [SerializeField]
    float _textAnimSpeed = 1.0f;

    [SerializeField]
    float _lastTextWaitTime = 0.5f;

    [Header("UI")]
    [SerializeField]
    Image _thankyouImage = default;

    [SerializeField]
    Image _circleImage = default;

    [SerializeField]
    Image _charaImage = default;

    [SerializeField]
    Image _developersImage = default;

    [SerializeField]
    Sprite _winkImage = default;

    Coroutine _fadeCol;

    void Start()
    {
        StartCoroutine(ClearDirection());
    }

    IEnumerator ClearDirection()
    {
        yield return ClearAnimation();

        yield return new WaitUntil(() => Input.anyKeyDown);

        _fadeCol = _fade.FadeIn(1.5f);

        yield return new WaitForSeconds(0.8f);

        StopCoroutine(_fadeCol);
        _fadeCol = null;

        yield return new WaitForSeconds(0.5f);

        Sprite originCharaImage = _charaImage.sprite;
        _charaImage.sprite = _winkImage;
        AudioManager.PlaySE(SEType.UI_Wink);

        yield return new WaitForSeconds(0.2f);

        _charaImage.sprite = originCharaImage;

        yield return new WaitForSeconds(1.0f);

        _fadeCol = _fade.FadeIn(0.5f, () =>
        {
            LoadSceneManager.Instance.LoadTitle();
        });
    }
    IEnumerator ClearAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        _circleImage.gameObject.transform
                               .DOScale(Vector3.one, _circleAnimSpeed)
                               .OnComplete(() =>
                               {
                                   _charaImage.gameObject.transform
                                                         .DOScale(Vector3.one, _charaAnimSpeed)
                                                         .SetEase(Ease.OutBounce)
                                                         .OnComplete(() =>
                                                         {
                                                             _thankyouImage.DOFillAmount(1, _textAnimSpeed)
                                                                           .SetEase(Ease.Linear);
                                                         });
                               });
        yield return new WaitForSeconds(_circleAnimSpeed + _charaAnimSpeed + _textAnimSpeed + _lastTextWaitTime);
        _developersImage.DOFillAmount(1, _textAnimSpeed).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2.0f);
    }
}
