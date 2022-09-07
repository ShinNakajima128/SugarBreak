using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// フィールドのクリームに触れた時にアニメーションを再生するクラス
/// </summary>
public class Cream : MonoBehaviour
{
    [SerializeField]
    Ease _creamAnimType = Ease.InOutBounce;

    [SerializeField]
    float _animValue = 0.3f;

    Transform _creamTrans;
    Vector3 _originScale;

    void Start()
    {
        _creamTrans = GetComponent<Transform>();
        _originScale = _creamTrans.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CreamController.Instance.OnCreamAnimation();
            CreamAnimation();
            VibrationController.OnVibration(Strength.Low, 0.2f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CreamController.Instance.OffCreamAnimation();
        }
    }

    void CreamAnimation()
    {
        _creamTrans.DOScale(_originScale.x + _animValue, 0.2f)
                   .SetEase(_creamAnimType)
                   .OnComplete(() => 
                   {
                       _creamTrans.DOScale(_originScale.x, 0.2f)
                                  .SetEase(_creamAnimType);
                   });
    }
}
