using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonMask : MonoBehaviour
{
    [SerializeField]
    float _durationTime = 1.0f;

    [SerializeField]
    Transform _targetTrans = default;

    Sequence seq;
    Vector3 _originPos;
    bool _init = false;

    void OnEnable()
    {
        if (!_init)
        {
            _originPos = transform.localPosition;
            _init = true;
        }

        OnMaskAnim();
    }

    void OnDisable()
    {
        FinishAnim();
    }

    void OnMaskAnim()
    {
        transform.localPosition = _originPos;

        seq = DOTween.Sequence();

        seq.Append(transform.DOMove(_targetTrans.position, _durationTime).SetEase(Ease.Linear))
           .OnComplete(() =>
           {
               transform.position = _originPos;
           })
           .SetLoops(-1)
           .Play();
    }

    void FinishAnim()
    {
        if (seq != null)
        {
            seq.Kill();
        }
    }
}
