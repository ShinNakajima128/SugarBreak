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
    [SerializeField] AnimationEventScript animationEventScript = null;
    /// <summary> 着地判定を取る距離 </summary>
    [SerializeField] float m_isGroundedLength = 0.05f;
    /// <summary> Effectを表示する場所 </summary>
    [SerializeField] Transform m_effectPos = null;

    [SerializeField]
    float m_minVelocityY = -4.5f;

    [SerializeField]
    float m_maxVelocityY = 2.5f;

    Dictionary<PlayerActions, Action> m_playerActionDic = new Dictionary<PlayerActions, Action>();
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
            //WeaponListManager.Instance.IconChange();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxisRaw("D Pad Ver") == 1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip2) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip2);
            WeaponChangeAction();
            //WeaponListManager.Instance.IconChange();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxisRaw("D Pad Hori") == -1)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip3) return;

            WeaponListControl.Instance.ChangeWeapon(WeaponListTypes.Equip3);
            WeaponChangeAction();
            //WeaponListManager.Instance.IconChange();
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
    /// キャンディビートの攻撃
    /// </summary>
    void AttackMove()
    {

        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Trigger") > 0)
        {
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip1)
            {
                if (IsGrounded())
                {
                    PlayerStatesManager.Instance.IsOperation = false;
                    m_anim.SetBool("Light", true);                      ///CandyBeatの弱攻撃
                    StartCoroutine(AttackMotionTimer(m_waitTime));
                }
                else
                {
                    PlayerStatesManager.Instance.IsOperation = false;
                    m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
                    m_rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
                    m_anim.SetBool("Strong", true);                     ///CandyBeatの強攻撃
                    StartCoroutine(AttackMotionTimer(m_waitTime));
                }
            }

            ///ポップランチャーの攻撃
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip2)
            {
                PlayerStatesManager.Instance.IsOperation = false;
                m_anim.SetBool("Shoot", true);
                StartCoroutine(AttackMotionTimer(m_waitTime));
                m_rb.velocity = new Vector3(0, m_rb.velocity.y, 0);
            }

            ///デュアルソーダの攻撃
            if (WeaponListControl.Instance.CurrentEquipWeapon == WeaponListTypes.Equip3)
            {
                if (comboNum == 3) return;

                if (comboNum == 0)
                {
                    m_anim.SetTrigger("SwordAttack1");
                    comboNum = 1;
                    combpCoroutine = StartCoroutine(AttackMotionTimer(0.3f));
                }
                else if (comboNum == 1)
                {
                    m_anim.SetTrigger("SwordAttack2");
                    comboNum = 2;
                    if (combpCoroutine != null)
                    {
                        StopCoroutine(combpCoroutine);
                        combpCoroutine = null;
                        combpCoroutine = StartCoroutine(AttackMotionTimer(0.3f));
                    }
                }
                else if (comboNum == 2)
                {
                    m_rb.velocity = Vector3.zero;
                    m_anim.SetTrigger("SwordAttack3");
                    PlayerStatesManager.Instance.IsOperation = false;
                    StartCoroutine(AttackMotionTimer(m_waitTime));
                    comboNum = 3;
                    if (combpCoroutine != null)
                    {
                        StopCoroutine(combpCoroutine);
                        combpCoroutine = null;
                        combpCoroutine = StartCoroutine(AttackMotionTimer(0.5f));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 武器種を変更した時のエフェクト表示やサウンド再生などのアクションを実行する
    /// </summary>
    void WeaponChangeAction()
    {
        EffectManager.PlayEffect(EffectType.ChangeWeapon, m_effectPos.position);
        SoundManager.Instance.PlaySeByName("Change");
        //WeaponListManager.Instance.IconChange();
    }

    /// <summary>
    /// 硬直
    /// </summary>
    /// <returns> </returns>
    IEnumerator AttackMotionTimer(float time)
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
