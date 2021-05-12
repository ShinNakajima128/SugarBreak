using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class InputTest : MonoBehaviour
{
    [SerializeField] float m_moveSpeed = 2f;
    [SerializeField] float m_jumpPower = 5f;
    CharacterController m_characterController;
    Vector3 m_velocity;
    PlayerInput m_playerInput;
    InputAction m_moveAction;
    InputAction m_jumpAction;
    InputAction m_attackAction;
    Animator m_anim;

    bool isOperation = true;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_playerInput = GetComponent<PlayerInput>();
        m_moveAction = m_playerInput.currentActionMap.FindAction("Move");
        m_jumpAction = m_playerInput.currentActionMap.FindAction("Jump");
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isOperation) return;

        if (m_characterController.isGrounded)
        {
            m_velocity = Vector3.zero;

            var input = new Vector3(m_moveAction.ReadValue<Vector2>().x, 0f, m_moveAction.ReadValue<Vector2>().y);

            if (input.magnitude > 0)
            {
                transform.LookAt(transform.position + input);
                m_velocity = transform.forward * m_moveSpeed;
                m_anim.SetFloat("Move", input.magnitude);
            }
            else
            {
                m_anim.SetFloat("Move", 0f);
            }

            if (m_jumpAction.triggered)
            {
                m_velocity.y += m_jumpPower;
                m_anim.SetBool("Jump", true);
            }
            else
            {
                m_anim.SetBool("Jump", false);
            }
        }
        m_velocity.y += Physics.gravity.y * Time.deltaTime;
        m_characterController.Move(m_velocity * Time.deltaTime);
    }
}
