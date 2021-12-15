using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActions 
{
    /// <summary> ジャンプ </summary>
    Jump,
    /// <summary> 攻撃処理開始 </summary>
    BeginAttack,
    /// <summary> 攻撃処理終了 </summary>
    FinishAttack,
    /// <summary> アイテム等の確認 </summary>
    Confirm,
}

[RequireComponent(typeof(CharacterController))]
public class PlayerActionControl : MonoBehaviour
{
    #region move
    [Header("移動")]
    [SerializeField]
    float m_moveSpeed = 8.0f;

    [SerializeField]
    float m_rotateSpeed = 5.0f;

    Transform m_selfTrans = default;
    Vector2 m_inputAxis = Vector2.zero;
    Vector3 m_moveForward = Vector3.zero;
    Vector3 m_currentVelocity = Vector3.zero;
    Quaternion m_targetRot = Quaternion.identity;
    #endregion

    #region jump
    [Header("ジャンプ")]
    [SerializeField]
    float m_jumpSpeed = 5.0f;

    [SerializeField]
    float m_gravityScale = 1.0f;
    #endregion

    Dictionary<PlayerActions, Action> m_playerActionDic = new Dictionary<PlayerActions, Action>();
    Animator m_anim = default;
    CharacterController m_charaCtrl = default;

    public static PlayerActionControl Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_selfTrans = transform;
        m_charaCtrl = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        var dir = new Vector3(m_inputAxis.x, 0f, m_inputAxis.y);
        m_moveForward = Camera.main.transform.TransformDirection(dir);

        ApplyInputAxis();
        ApplyGravity();
        ApplyMovement();
        ApplyRotation();
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

    /// <summary>
    /// アクションを登録する
    /// </summary>
    /// <param name="actions"> アクションの種類 </param>
    /// <param name="action"> 追加する処理 </param>
    public static void ListenActions(PlayerActions actions, Action action)
    {
        if (Instance == null) return;
        Action thisEvent;
        if (Instance.m_playerActionDic.TryGetValue(actions, out thisEvent))
        {
            thisEvent += action;

            Instance.m_playerActionDic[actions] = thisEvent;
        }
        else
        {
            thisEvent += action;
            Instance.m_playerActionDic.Add(actions, thisEvent);
        }
    }

    /// <summary>
    /// 登録したアクションを抹消する
    /// </summary>
    /// <param name="events"> アクションの種類 </param>
    /// <param name="action"> 抹消する処理 </param>
    public static void RemoveActions(PlayerActions events, Action action)
    {
        if (Instance == null) return;
        Action thisEvent;
        if (Instance.m_playerActionDic.TryGetValue(events, out thisEvent))
        {
            thisEvent -= action;

            Instance.m_playerActionDic[events] = thisEvent;
        }
    }

    /// <summary>
    /// アクションを実行する
    /// </summary>
    /// <param name="events"> 実行するアクション </param>
    public static void OnAction(PlayerActions events)
    {
        Action thisEvent;
        if (Instance.m_playerActionDic.TryGetValue(events, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }
}

