using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamepadKeyConfig
{
    public KeyCode JumpKey;
    public KeyCode AttackKey;
    public KeyCode AvoidKey;
}

[Serializable]
public class KeyboardMouseKeyConfig
{
    public KeyCode JumpKey;
    public KeyCode AttackKey;
    public KeyCode AvoidKey;
}

public class KeyConfigManager : MonoBehaviour
{
    [SerializeField]
    GamepadKeyConfig m_gamepadConfig = default;

    [SerializeField]
    KeyboardMouseKeyConfig m_keyboardMouseKeyConfig = default;

    public static KeyConfigManager Instance { get; private set; }
    public bool JumpInputDown
    {
        get 
        {
            if (Input.GetKeyDown(m_gamepadConfig.JumpKey) || Input.GetKeyDown(m_keyboardMouseKeyConfig.JumpKey))
            {
                Debug.Log("ジャンプ");
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
}
