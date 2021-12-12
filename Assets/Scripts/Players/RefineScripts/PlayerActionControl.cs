using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates 
{
    None,
    Move,
    Dead
}

[RequireComponent(typeof(MoveBehavior))]
[RequireComponent(typeof(JumpBehavior))]
[RequireComponent(typeof(CharacterController))]
public class PlayerActionControl : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed = 8.0f;

    [SerializeField]
    float m_rotateSpeed = 5.0f;

    [SerializeField]
    float m_jumpSpeed = 5.0f;

    [SerializeField]
    float m_gravityScale = 1.0f;

    Transform m_selfTrans = default;
    Animator m_anim = default;
    CharacterController m_charaCtrl = default;

    Vector2 m_inputAxis = Vector2.zero;
    Vector3 m_moveForward = Vector3.zero;
    Vector3 m_currentVelocity = Vector3.zero;
    Quaternion m_targetRot = Quaternion.identity;

    public Action PlayerMove = default;


    void Start()
    {
        m_selfTrans = transform;
        m_charaCtrl = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    void ApplyInputAxis()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        m_inputAxis = new Vector2(h, v);
    }

    void ApplyMovement()
    {
        var velocity = Vector3.Scale(m_currentVelocity, new Vector3(m_moveSpeed, 1f, m_moveSpeed));
        m_charaCtrl.Move(Time.deltaTime * velocity);
    }

    void ApplyRotation()
    {
        var rot = m_selfTrans.rotation;
        rot = Quaternion.Slerp(rot, m_targetRot, m_rotateSpeed * Time.deltaTime);
        m_selfTrans.rotation = rot;
    }

    void ApplyGravity()
    {
        if (!m_charaCtrl.isGrounded)
        {
            m_currentVelocity.y += m_gravityScale * Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            m_currentVelocity.y = 0f;
        }
    }

    void PlayAnimation(string stateName, float transitionDuration = 0.1f)
    {
        m_anim.CrossFadeInFixedTime(stateName, transitionDuration);
    }
}
