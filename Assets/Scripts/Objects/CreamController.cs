using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreamController : MonoBehaviour
{
    [SerializeField]
    float _animSpeed = 2.0f;

    [SerializeField]
    float _moveValue = 30f;

    [SerializeField]
    RectTransform _creamTrans = default;

    [SerializeField]
    Sprite[] _creamSprites = default;

    Vector3 _originPos;
    bool _isPlayed = false;
    Image _creamImage;
    Sequence _seq;

    public static CreamController Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _creamImage = _creamTrans.gameObject.GetComponent<Image>();
        _originPos = _creamTrans.localPosition;
    }

    public void OnCreamAnimation()
    {
        if (_seq != null)
        {
            //StopCoroutine(_offCreamCor);
            //_offCreamCor = null;
            _seq.Kill();
            _seq = null;
            _isPlayed = false;
        }
        StartCoroutine(OnCream());
    }

    public void OffCreamAnimation()
    {
        StartCoroutine(OffCream());
    }

    IEnumerator OnCream()
    {
        Debug.Log("クリームに当たった");
        
        while (true)
        {
            int r = Random.Range(0, _creamSprites.Length);

            if (_creamImage.sprite != _creamSprites[r])
            {
                _creamImage.sprite = _creamSprites[r];
                Debug.Log("決定");
                break;
            }
            //yield return null;
        }
        

        _isPlayed = true;
        _creamImage.enabled = true;
        _creamTrans.localPosition = _originPos;
        _creamImage.DOFade(1, 0);

        AudioManager.PlaySE(SEType.Cream);
        yield return null;
    }
    IEnumerator OffCream()
    {
        if (!_isPlayed)
        {
            yield break;
        }
        _seq = DOTween.Sequence();
        _seq.Append(_creamTrans.DOLocalMoveY(_creamTrans.localPosition.y - _moveValue, _animSpeed))
            .Join(_creamImage.DOFade(0, _animSpeed))
            .OnComplete(() =>
            {
                _isPlayed = false;
                _creamImage.enabled = false;
            })
            .Play();

        //_creamTrans.DOLocalMoveY(_creamTrans.localPosition.y - _moveValue, _animSpeed);
        //_creamImage.DOFade(0, _animSpeed)
        //           .OnComplete(() =>
        //           {
        //               _isPlayed = false;
        //               _creamImage.enabled = false;
        //               _offCreamCor = null;
        //           });
    }
}
