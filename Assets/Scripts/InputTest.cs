using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour, ExampleControls.IPlayerActions
{
    ExampleControls m_input;

    void Awake()
    {
        m_input = new ExampleControls();

        m_input.Player.SetCallbacks(this);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Fire");
        }
    }

    void OnEnable()
    {
        m_input.Enable();
    }

    void OnDisable()
    {
        m_input.Disable();
    }

    void OnDestroy()
    {
        m_input.Dispose();
    }
}
