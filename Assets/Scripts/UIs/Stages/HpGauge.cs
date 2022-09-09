using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class HpGauge : MonoBehaviour
{
    [Header("偶数体力")]
    [SerializeField]
    Image[] _evenHpObj = default;

    [Header("奇数体力")]
    [SerializeField]
    Image[] _addHpObj = default;

    /// <summary> プレイヤーのデータ </summary>
    [SerializeField]
    PlayerData playerData = default;

    [SerializeField]
    Transform _hpListTrans = default;

    List<Image> _currentHpList = new List<Image>();
    Vector3 _originListPosition;

    void Start()
    {
        _originListPosition = _hpListTrans.position;
        Debug.Log(_originListPosition);
    }

    /// <summary>
    /// HPの数値に応じて体力オブジェクトをセットする
    /// </summary>
    /// <param name="hp"> HPの値 </param>
    public void SetHpGauge(int hp, bool isDamaged = false)
    {
        ///　体力を一旦全削除
        for (int i = 0; i < _hpListTrans.childCount; i++)
        {
            Destroy(_hpListTrans.GetChild(i).gameObject);
            
        }
        _currentHpList.Clear();

        for (int i = 1; i <= hp; i++)
        {
            if (i == hp && i % 2 == 1)
            {
                if (i == 7)
                {
                    _currentHpList.Add(Instantiate(_addHpObj[0], _hpListTrans));
                }
                else if (i == 5)
                {
                    _currentHpList.Add(Instantiate(_addHpObj[2], _hpListTrans));

                }
                else if (i == 3)
                {
                    _currentHpList.Add(Instantiate(_addHpObj[1], _hpListTrans));

                }
                else if (i == 1)
                {
                    _currentHpList.Add(Instantiate(_addHpObj[0], _hpListTrans));
                }

            }
            else if (i % 2 == 0)
            {
                if (i == 8)
                {
                    _currentHpList.Add(Instantiate(_evenHpObj[0], _hpListTrans));
                }
                else if (i == 6)
                {
                    _currentHpList.Add(Instantiate(_evenHpObj[2], _hpListTrans));

                }
                else if (i == 4)
                {
                    _currentHpList.Add(Instantiate(_evenHpObj[1], _hpListTrans));

                }
                else if (i == 2)
                {
                    _currentHpList.Add(Instantiate(_evenHpObj[0], _hpListTrans));
                }
            }
        }

        if (hp > 2)
        {
            _currentHpList[_currentHpList.Count - 1].gameObject.transform.DOScale(1.2f, 0.5f)
                                                                         .SetLoops(-1, LoopType.Yoyo);
        }
        else if (hp <= 2 && hp > 0)
        {
            _currentHpList[_currentHpList.Count - 1].DOColor(Color.red, 0.5f)
                                                    .SetLoops(-1, LoopType.Yoyo);
            _currentHpList[_currentHpList.Count - 1].gameObject.transform.DOScale(1.2f, 0.5f)
                                                                         .SetLoops(-1, LoopType.Yoyo);
        }
        
        if (isDamaged)
        {
            _hpListTrans.DOShakePosition(0.25f, 10, 20)
                        .OnComplete(() => 
                        {
                            _hpListTrans.position = _originListPosition;
                        });
        }
    }
}
