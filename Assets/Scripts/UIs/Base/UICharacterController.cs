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
/// 武器メニュー画面のキャラクター
/// </summary>
public class UICharacterController : MonoBehaviour
{
    [Tooltip("モデルの回転角度")]
    [SerializeField]
    float m_rotateAngle = 30f;

    Transform _characterTrans;
    bool _init = false;

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

        if (!_init)
        {
            WeaponMenuManager.Instance.OnRotateAction += CharacterRotate;
            _init = true;
        }
    }

    void CharacterRotate(RotateType type)
    {
        switch (type)
        {
            case RotateType.Left:
                _characterTrans.Rotate(new Vector3(0, m_rotateAngle, 0) * Time.deltaTime);
                break;
            case RotateType.Right:
                _characterTrans.Rotate(new Vector3(0, -m_rotateAngle, 0) * Time.deltaTime);
                break;
        }
    }
}
