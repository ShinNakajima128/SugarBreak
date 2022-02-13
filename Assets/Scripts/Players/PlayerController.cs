using System;
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
    /// <summary> 着地判定を取る距離 </summary>
    [SerializeField] float m_isGroundedLength = 0.05f;
    /// <summary> Effectを表示する場所 </summary>
    [SerializeField] Transform m_effectPos = null;

    [SerializeField]
    float m_minVelocityY = -4.5f;

    [SerializeField]
    float m_maxVelocityY = 2.5f;

    [Header("回避モーションの各数値")]
    /// <summary> 回避中にかけられる力の数値 </summary>
    [SerializeField]
    float m_pushPower = 10.0f;
    /// <summary> 徐々に力を減らす数値 </summary>
    [SerializeField]
    float m_decreaseValue = 10.0f;

    PlayerState state = PlayerState.None;
    Rigidbody m_rb;
    Animator m_anim;
    bool m_isDodged = false;
    bool m_isAttackMotioned = false;
    bool m_isAimed = false;
    bool m_isAimMoved = false;
    bool m_isJumped = false;
    float actualPushPower;

    public PlayerState State
    {
        get { return state; }
        set { state = value; }
    }
    public float RunSpeed => m_runSpeed;
    public bool IsDodged => m_isDodged;
    public bool IsAimed { get => m_isAimed; set => m_isAimed = value; }
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
        actualPushPower = m_pushPower;
    }

    void Update()
    {
        ///Playerが操作可能だったら
        if (PlayerStatesManager.Instance.IsOperation)
        {
            PlayerMove();

            AttackMove();

            JumpMove();

            if (!m_isAttackMotioned)
            {
                WeaponChange();
            }

            DodgeMove();

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
        if (m_isDodged)
        {
            m_rb.AddForce(transform.forward * actualPushPower, ForceMode.Force);
            if (actualPushPower >= 0)
            {
                actualPushPower -= m_decreaseValue * Time.deltaTime;
            }
        }
    }

    void DodgeMove()
    {
        if (Input.GetKeyDown(KeyCode.Q) && IsGrounded() && !IsAimed)
        {
            if (m_rb.velocity == new Vector3(0f, m_rb.velocity.y, 0f))
            {
                return;
            }

            if (!m_isDodged)
            {
                StartCoroutine(Dodge());
                m_isDodged = true;
                SoundManager.Instance.PlayVoiceByName("univ0005");
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
        Vector3 start1 = transform.position;   // start: オブジェクトの中心
        Vector3 end1 = start1 + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start1, end1); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start1, end1); // 引いたラインに何かがぶつかっていたら true とする
        if (isGrounded)
        {
            m_anim.SetBool("isGround", true);
        }
        else
        {
            m_anim.SetBool("isGround", false);
        }
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
            if (m_isAimed && m_isAimMoved)
            {
                m_anim.SetFloat("ForwardMove", 0);
                m_anim.SetFloat("BackwardMove", 0);
                m_anim.SetFloat("RightMove", 0);
                m_anim.SetFloat("LeftMove", 0);
                m_isAimMoved = false;
            }
        }
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする


            if (!IsAimed)
            {
                // 入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);
            }

            if (dir != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift) || IsAimed)
                {
                    Vector3 velo = dir.normalized * m_walkSpeed; // 入力した方向に移動する
                    velo.y = WallHit ? -9.8f : m_rb.velocity.y;   // 壁に当たっているかどうかでVelocityを調整
                    m_rb.velocity = velo;
                    state = PlayerState.Walk;
                }
                else
                {
                    Vector3 velo = dir.normalized * m_runSpeed; // 入力した方向に移動する
                    velo.y = WallHit ? -9.8f : m_rb.velocity.y;   // 壁に当たっているかどうかでVelocityを調整

                    m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
                    state = PlayerState.Run;
                }
            }

            //精密射撃モードの場合
            if (IsAimed)
            {
                if (!m_isAimMoved)
                {
                    m_isAimMoved = true;
                }

                //前に進むアニメーション
                if (dir.z >= 0.9f)
                {
                    m_anim.SetFloat("ForwardMove", 2.0f);
                    m_anim.SetFloat("BackwardMove", 0);
                    m_anim.SetFloat("RightMove", 0);
                    m_anim.SetFloat("LeftMove", 0);
                }
                //後ろに進むアニメーション
                else if (dir.z <= -0.9f)
                {
                    m_anim.SetFloat("ForwardMove", 0);
                    m_anim.SetFloat("BackwardMove", 2.0f);
                    m_anim.SetFloat("RightMove", 0);
                    m_anim.SetFloat("LeftMove", 0);
                }
                //左に進むアニメーション
                else if (dir.x >= 0.9f)
                {
                    m_anim.SetFloat("ForwardMove", 0);
                    m_anim.SetFloat("BackwardMove", 0);
                    m_anim.SetFloat("RightMove", 2);
                    m_anim.SetFloat("LeftMove", 0);
                }
                //右に進むアニメーション
                else if (dir.x <= -0.9f)
                {
                    m_anim.SetFloat("ForwardMove", 0);
                    m_anim.SetFloat("BackwardMove", 0);
                    m_anim.SetFloat("RightMove", 0);
                    m_anim.SetFloat("LeftMove", 2);
                }
            }
        }
    }

    /// <summary>
    /// 急降下する。アニメーションイベントにて使用
    /// </summary>
    public void FallDown()
    {
        m_rb.AddForce(Vector3.down * 35, ForceMode.Impulse);
        EventManager.OnEvent(Events.CameraShake);
    }

    /// <summary>
    /// ジャンプモーションを開始する
    /// </summary>
    public void JumpMotion()
    {
        StartCoroutine(Jump());
    }

    /// <summary>
    /// 武器を切り替える
    /// </summary>
    public void WeaponChange()
    {
        //ADS中は武器変更不可
        if (m_isAimed)
        {
            return;
        }

        //キーボードの「1」かゲームパッドの十字キー「左」を押したら「装備1」に変更
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxisRaw("D Pad Hori") == 1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip1) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip1);
            WeaponChangeAction();
            AimRotation.Instance.ResetWeaponListRotation();
            m_anim.Rebind();
        }
        //キーボードの「2」かゲームパッドの十字キー「上」を押したら「装備2」に変更
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxisRaw("D Pad Ver") == 1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip2) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip2);
            WeaponChangeAction();
            m_anim.Rebind();
            AimRotation.Instance.ResetWeaponListRotation();
        }
        //キーボードの「3」かゲームパッドの十字キー「右」を押したら「装備3」に変更
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxisRaw("D Pad Hori") == -1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip3) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip3);
            WeaponChangeAction();
            m_anim.Rebind();
            AimRotation.Instance.ResetWeaponListRotation();
        }
    }

    /// <summary>
    /// ジャンプする。アニメーションイベントに設定
    /// </summary>
    public void JumpUp()
    {
        m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    void JumpMove()
    {
        if (Input.GetButtonDown("Jump") && !m_isJumped)
        {
            //接地していたらジャンプ
            if (IsGrounded())
            {
                JumpMotion();
                m_anim.SetBool("Jump", true);
                SoundManager.Instance.PlayVoiceByName("univ0001");
            }
        }
    }

    /// <summary>
    /// 各武器での攻撃アクション
    /// </summary>
    void AttackMove()
    {
        //通常攻撃ボタンを押したら
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Trigger") > 0)
        {
            //地上にいる時の攻撃
            if (IsGrounded())
            {
                CurrentWeaponAction.WeaponAction1(m_anim, m_rb);
            }
            //空中時の攻撃
            else
            {
                CurrentWeaponAction.WeaponAction2(m_anim, m_rb);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            CurrentWeaponAction.WeaponAction3(m_anim, m_rb);
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
    public IEnumerator AttackMotionTimer(float time, Action comboResetCallBack = null)
    {
        PlayerStatesManager.Instance.IsOperation = false;

        m_isAttackMotioned = true;
        m_anim.SetFloat("Move", 0);
        yield return new WaitForSeconds(time);
        PlayerStatesManager.Instance.IsOperation = true;
        yield return new WaitForSeconds(time + 0.5f);

        comboResetCallBack?.Invoke();
        m_isAttackMotioned = false;
    }

    IEnumerator Jump()
    {
        m_isJumped = true;
        yield return new WaitForSeconds(0.2f);

        //m_anim.SetBool("isGround", false);
        m_isJumped = false;
    }

    /// <summary>
    /// 回避モーション
    /// </summary>
    IEnumerator Dodge()
    {
        PlayerStatesManager.Instance.IsOperation = false;
        m_anim.SetTrigger("isDodged");
        yield return new WaitForSeconds(1.0f);
        actualPushPower = m_pushPower;
        m_isDodged = false;
        PlayerStatesManager.Instance.IsOperation = true;
    }
}
