using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClearPanelController : MonoBehaviour
{
    [SerializeField]
    float _animSpeed = 0.25f;

    [Header("星")]
    [SerializeField]
    RectTransform[] _stars = default;

    [SerializeField]
    float _starAnimSpeed = 2.0f;

    [SerializeField]
    Ease _starEaseType = Ease.InOutQuad;

    [Header("飾り")]
    [SerializeField]
    RectTransform _rightUpper = default;

    [SerializeField]
    RectTransform _rightlower = default;

    [SerializeField]
    RectTransform _leftLower = default;

    [SerializeField]
    float _decorationsAnimSpeed = 0.5f;

    [SerializeField]
    float _moveValue = 10f;

    [SerializeField]
    Ease _decoEaseType = Ease.Linear;

    [Header("ステージクリア")]
    [SerializeField]
    RectTransform _emission = default;

    [SerializeField]
    float _emissionRotateSpeed = 6.0f;

    [SerializeField]
    RectTransform _stageClear = default;

    [SerializeField]
    float _stageClearAnimSpeed = 1.0f;

    [SerializeField]
    Ease _stageClearEaseType = Ease.InBounce;

    [Header("ラベル")]
    [SerializeField]
    RectTransform _clearFlame = default;

    [SerializeField]
    float _flameWaitTime = 0.8f;

    [SerializeField]
    float _flameMoveSpeed = 0.25f;

    [SerializeField]
    Ease _flameEaseType = default;

    [Header("フラッシュ")]
    [SerializeField]
    Image _flash = default;

    [SerializeField]
    float _flashFadeInTime = 0.05f;

    [SerializeField]
    float _flashFadeOutTime = 1.0f;

    [SerializeField]
    Image _PanelBackground = default;

    void Start()
    {
        StartCoroutine(OnClearPanelAnimation());
    }

    /// <summary>
    /// クリア画面のアニメーションを再生
    /// </summary>
    IEnumerator OnClearPanelAnimation()
    {
        _flash.DOFade(1, _flashFadeInTime)
              .OnComplete(() =>
              {
                  _PanelBackground.enabled = true;
                  _flash.DOFade(0, _flashFadeOutTime);
              });

        //各星Imageのアニメーション
        foreach (var s in _stars)
        {
            s.DOScale(0.9f, _starAnimSpeed)
             .SetEase(_starEaseType)
             .SetLoops(-1, LoopType.Yoyo);
        }

        yield return new WaitForSeconds(0.25f);

        //ステージクリアの後ろのImageのアニメーション
        _emission.DOScale(1.0f, _stageClearAnimSpeed);

        //ステージクリアImageのアニメーション
        _stageClear.DOScale(1.0f, _stageClearAnimSpeed)
                   .SetEase(_stageClearEaseType);

        yield return new WaitForSeconds(0.25f);

        //右上のImageのアニメーション
        _rightUpper.DORotate(Vector3.zero, _animSpeed)
                   .OnComplete(() =>
                   {
                       //出現アニメーション後に上下に動くアニメーションを再生
                       _rightUpper.DOLocalMoveY(_rightUpper.transform.localPosition.y + _moveValue, _decorationsAnimSpeed)
                                  .SetEase(_decoEaseType)
                                  .SetLoops(-1, LoopType.Yoyo);
                   });

        //右下のImageのアニメーション
        _rightlower.DORotate(Vector3.zero, _animSpeed)
                   .OnComplete(() =>
                   {
                       //出現アニメーション後に上下に動くアニメーションを再生
                       _rightlower.DOLocalMoveY(_rightlower.transform.localPosition.y - _moveValue, _decorationsAnimSpeed)
                                  .SetEase(_decoEaseType)
                                  .SetLoops(-1, LoopType.Yoyo);
                   });

        //左下のImageのアニメーション
        _leftLower.DORotate(Vector3.zero, _animSpeed)
                  .OnComplete(() =>
                  {
                      //出現アニメーション後に上下に動くアニメーションを再生
                      _leftLower.DOLocalMoveY(_leftLower.transform.localPosition.y - _moveValue, _decorationsAnimSpeed)
                                .SetEase(_decoEaseType)
                                .SetLoops(-1, LoopType.Yoyo);
                  });

        _emission.DORotate(new Vector3(0, 0, -360), _emissionRotateSpeed, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1);

        yield return new WaitForSeconds(_flameWaitTime);

        _clearFlame.DOLocalMoveX(544, _flameMoveSpeed)
                   .SetEase(_flameEaseType);
    }
}
