using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum IconType
{
    A,
    B,
    X,
    Y,
    LB,
    LT,
    Option,
    LStick,
    RStick
}
public class OperationController : MonoBehaviour
{
    [SerializeField]
    Color _animColor = default;

    [SerializeField]
    Color _originColor = Color.white;

    [SerializeField]
    List<OperationIcon> _icons = default;

    bool _isTrigger = false;

    public static OperationController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            OnIconAnimation(IconType.A);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            OffIconAnimation(IconType.A);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            OnIconAnimation(IconType.B);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            OffIconAnimation(IconType.B);
        }
       
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            OnIconAnimation(IconType.X);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button2))
        {
            OffIconAnimation(IconType.X);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            OnIconAnimation(IconType.Y);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button3))
        {
            OffIconAnimation(IconType.Y);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            OnIconAnimation(IconType.LB);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button4))
        {
            OffIconAnimation(IconType.LB);
        }

        if (Input.GetAxisRaw("L_Trigger") > 0)
        {
            if (!_isTrigger)
            {
                OnIconAnimation(IconType.LT);
                _isTrigger = true;
            }
        }
        else if (Input.GetAxisRaw("L_Trigger") == 0)
        {
            if (_isTrigger)
            {
                OffIconAnimation(IconType.LT);
                _isTrigger = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            OnIconAnimation(IconType.Option);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button7))
        {
            OffIconAnimation(IconType.Option);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button8))
        {
            OnIconAnimation(IconType.LStick);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button8))
        {
            OffIconAnimation(IconType.LStick);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            OnIconAnimation(IconType.RStick);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button9))
        {
            OffIconAnimation(IconType.RStick);
        }
    }

    void OnIconAnimation(IconType type)
    {
        var icon = _icons.FirstOrDefault(i => i.Type == type);

        if (icon != default)
        {
            icon.Icon.color = _originColor;
            icon.Icon.DOColor(_animColor, 0.15f);
        }
        else
        {
            Debug.Log("Iconが見つかりませんでした");
        }
    }
    void OffIconAnimation(IconType type)
    {
        var icon = _icons.FirstOrDefault(i => i.Type == type);

        if (icon != default)
        {
            icon.Icon.DOColor(_originColor, 0.15f);
        }
        else
        {
            Debug.Log("Iconが見つかりませんでした");
        }
    }
}
[Serializable]
public class OperationIcon
{
    public string Name;
    public IconType Type;
    public Image Icon;
}
