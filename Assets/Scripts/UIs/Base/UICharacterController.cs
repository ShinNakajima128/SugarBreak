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

    #region private
    Transform _characterTrans;
    Animator _animator;
    bool _init = false;
    Quaternion _originRotation;
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
            _init = true;
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
}
