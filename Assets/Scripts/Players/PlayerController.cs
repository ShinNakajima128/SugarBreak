﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステータス
/// </summary>
public enum PlayerState
{
    None,
    Idle,
    Walk,
    Run
}

/// <summary>
/// プレイヤー操作の機能を持つクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float m_walkSpeed = 5f;
    /// <summary>動く速さ</summary>
    [SerializeField] float m_runSpeed = 5f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary> 硬直時間 </summary>
    [SerializeField] float m_waitTime = 1.0f;
    [SerializeField] AnimationEventScript animationEventScript = null;
    /// <summary> 着地判定を取る距離 </summary>
    [SerializeField] float m_isGroundedLength = 0.05f;
    /// <summary> Effectを表示する場所 </summary>
    [SerializeField] Transform m_effectPos = null;

    [SerializeField]
    float m_minVelocityY = -4.5f;

    [SerializeField]
    float m_maxVelocityY = 2.5f;

    PlayerState state = PlayerState.None;
    Rigidbody m_rb;
    Animator m_anim;
    int comboNum = 0;
    Coroutine combpCoroutine;
    bool isSliding;

    public PlayerState State
    {
        get { return state; }
        set { state = value; }
    }
    public float RunSpeed => m_runSpeed;
    public bool WallHit { get; set; } = false;
    public static PlayerController Instance { get; private set; }
    public IWeapon CurrentWeaponAction { get; set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        ///Playerが操作可能だったら
        if (PlayerStatesManager.Instance.IsOperation)
        {
            if (comboNum <= 0)
            {
                PlayerMove();
            }

            AttackMove();

            JumpMove();

            WeaponChange();

            if (m_anim)
            {
                if (IsGrounded())
                {
                    Vector3 velo = m_rb.velocity;
                    velo.y = 0;
                    if (state == PlayerState.Walk)
                    {
                        m_anim.SetFloat("Move", velo.magnitude);
                    }
                    else if (state == PlayerState.Run)
                    {
                        m_anim.SetFloat("Move", velo.magnitude);
                    }
                    else if (state == PlayerState.Idle)
                    {
                        m_anim.SetFloat("Move", 0f);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 着地しているかどうか
    /// </summary>
    /// <returns> 着地判定 </returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        if (isGrounded) m_anim.SetBool("isGround", true);
        return isGrounded;
    }

    /// <summary>
    /// プレイヤーの操作
    /// </summary>
    void PlayerMove()
    {
        // 方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        Vector3 dir = Vector3.forward * v + Vector3.right * h;

        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
            state = PlayerState.Idle;
        }
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);  // Slerp を使うのがポイント

            if (dir != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Vector3 velo = dir.normalized * m_walkSpeed; // 入力した方向に移動する
                    velo.y = Mathf.Clamp(m_rb.velocity.y, m_minVelocityY, m_maxVelocityY); ;   // ジャンプした時の y 軸方向の速度を保持する
                    m_rb.velocity = velo;
                    state = PlayerState.Walk;
                }
                else
                {
                    Vector3 velo = dir.normalized * m_runSpeed; // 入力した方向に移動する
                    velo.y = Mathf.Clamp(m_rb.velocity.y, m_minVelocityY, m_maxVelocityY);   // ジャンプした時の y 軸方向の速度を保持する
                    
                    m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
                    state = PlayerState.Run;
                }
            }
        }
    }

    /// <summary>
    /// 急降下する。アニメーションイベントにて使用
    /// </summary>
    public void FallDown()
    {
        if (!IsGrounded())
        {
            m_rb.AddForce(Vector3.down * 35, ForceMode.Impulse);
        }
    }

    public void JumpMotion()
    {
        StartCoroutine(Jump());
        m_anim.SetBool("Jump", true);
        m_anim.SetBool("isGround", false);
    }

    /// <summary>
    /// 武器を切り替える
    /// </summary>
    public void WeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxisRaw("D Pad Hori") == 1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip1) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip1);
            WeaponChangeAction();
            m_anim.Rebind();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxisRaw("D Pad Ver") == 1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip2) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip2);
            WeaponChangeAction();
            m_anim.Rebind();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxisRaw("D Pad Hori") == -1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip3) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip3);
            WeaponChangeAction();
            m_anim.Rebind();
        } 
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    void JumpMove()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
                JumpMotion();
                SoundManager.Instance.PlayVoiceByName("univ0001");
            }
        }
    }

    /// <summary>
    /// 各武器での攻撃アクション
    /// </summary>
    void AttackMove()
    {
        //通常攻撃
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Trigger") > 0)
        {
            //地上にいる時の攻撃
            if (IsGrounded())
            {
                CurrentWeaponAction.WeaponAction1(m_anim, m_rb, combpCoroutine, comboNum);
                StartCoroutine(AttackMotionTimer(m_waitTime));
            }
            //空中時の攻撃
            else
            {
                CurrentWeaponAction.WeaponAction2(m_anim, m_rb);
                StartCoroutine(AttackMotionTimer(m_waitTime));
            }
            PlayerStatesManager.Instance.IsOperation = false;
        }
    }

    /// <summary>
    /// 武器種を変更した時のエフェクト表示やサウンド再生などのアクションを実行する
    /// </summary>
    void WeaponChangeAction()
    {
        EffectManager.PlayEffect(EffectType.ChangeWeapon, m_effectPos.position);
        SoundManager.Instance.PlaySeByName("Change");
    }

    /// <summary>
    /// 硬直
    /// </summary>
    /// <returns> </returns>
    public IEnumerator AttackMotionTimer(float time)
    {
        m_anim.SetFloat("Move", 0);
        yield return new WaitForSeconds(time);

        PlayerStatesManager.Instance.IsOperation = true;

        if (comboNum != 0)
        {
            yield return new WaitForSeconds(time);

            comboNum = 0;

            m_anim.SetBool("SwordAttack1", false);
            m_anim.SetBool("SwordAttack2", false);
            m_anim.SetBool("SwordAttack3", false);
        }
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.3f);

        m_anim.SetBool("isGround", false);
    }
}
