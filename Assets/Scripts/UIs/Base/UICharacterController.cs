using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転方向の種類
/// </summary>
public enum RotateType
{
    Left,
    Right
}

/// <summary>
/// アニメーションの種類
/// </summary>
public enum AnimationType
{

}

/// <summary>
/// 武器メニュー画面のキャラクター
/// </summary>
public class UICharacterController : MonoBehaviour
{
    [Tooltip("モデルの回転角度")]
    [SerializeField]
    float _rotateAngle = 30f;

    [Header("WeaponObjects")]
    [SerializeField]
    GameObject[] _weaponObjects = default;

    #region private
    Transform _characterTrans;
    Animator _animator;
    bool _init = false;
    Quaternion _originRotation;
    Dictionary<WeaponTypes, GameObject> _weaponObjectDic = new Dictionary<WeaponTypes, GameObject>();
    #endregion

    void OnEnable()
    {
        if (_init)
        {
            WeaponMenuManager.Instance.OnRotateAction += CharacterRotate;
        }
    }

    void OnDisable()
    {
        WeaponMenuManager.Instance.OnRotateAction -= CharacterRotate;
    }

    void Start()
    {
        _characterTrans = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _originRotation = transform.localRotation;

        if (!_init)
        {
            WeaponMenuManager.Instance.OnRotateAction += CharacterRotate;
            WeaponMenuManager.Instance.OnActiveAction += ResetCharacterRotation;
            WeaponMenuManager.Instance.OnWeaponButtonClickAction += WeaponActivation;
            _init = true;
        }

        //武器オブジェクトをDictionaryに登録
        for (int i = 0; i < _weaponObjects.Length; i++)
        {
            WeaponTypes type = (WeaponTypes)(i + 1);
            _weaponObjectDic.Add(type, _weaponObjects[i]);
        }
    }

    /// <summary>
    /// 装備メニューのキャラクターを回転させる
    /// </summary>
    /// <param name="type"> 回転方向 </param>
    void CharacterRotate(RotateType type)
    {
        switch (type)
        {
            case RotateType.Left:
                _characterTrans.Rotate(new Vector3(0, _rotateAngle, 0) * Time.deltaTime);
                break;
            case RotateType.Right:
                _characterTrans.Rotate(new Vector3(0, -_rotateAngle, 0) * Time.deltaTime);
                break;
        }
    }

    /// <summary>
    /// キャラクターモデルの回転状況をリセットする
    /// </summary>
    void ResetCharacterRotation()
    {
        transform.localRotation = _originRotation;
    }

    /// <summary>
    /// キャラクターの武器をアクティブにする
    /// </summary>
    /// <param name="weapon"> 武器のデータ </param>
    void WeaponActivation(WeaponData weapon)
    {
        //一度すべて非アクティブにする
        foreach (var wo in _weaponObjects)
        {
            wo.SetActive(false);
        }

        //指定された武器が開放されている場合はアクティブにする
        if (weapon.IsUnrocked)
        {
            _weaponObjectDic[weapon.WeaponType].SetActive(true);
        }
    }
}
