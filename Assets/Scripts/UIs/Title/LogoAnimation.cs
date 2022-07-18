using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ロゴのアニメーション機能を持つコンポーネント
/// </summary>
public class LogoAnimation : MonoBehaviour
{
    [Tooltip("アニメ―ションの移動幅の値")]
    [SerializeField]
    float _animRangeValue = 2.0f;

    [Tooltip("アニメーションにかける時間")]
    [SerializeField]
    float _durationTime = 0.5f;

    [Tooltip("アニメーションの動き方")]
    [SerializeField]
    Ease _animEase = default;

    Vector3 _originPos;
    Vector3 _afterPos;

    void Start()
    {
        _originPos = transform.localPosition;
        _afterPos = _originPos + new Vector3(0, _animRangeValue, 0);
        OnLogoAnim();
    }

    /// <summary>
    /// ロゴのアニメーションを再生
    /// </summary>
    void OnLogoAnim()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMove(_afterPos, _durationTime).SetEase(_animEase))
           .Append(transform.DOLocalMove(_originPos, _durationTime).SetEase(_animEase))
           .SetLoops(-1)
           .Play();
    }
}
