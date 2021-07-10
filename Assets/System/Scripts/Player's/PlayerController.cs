using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum PlayerState
{
    None,
    Idle,
    Walk,
    Run
}

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
    [SerializeField] bool m_shabadubiMode = false;
    PlayerState state = PlayerState.None;
    Rigidbody m_rb;
    Animator m_anim;
    SoundManager soundManager = default;
    [SerializeField] CinemachineFreeLook freeLook = default;
    /// <summary> 落下速度の下限 </summary>
    float minVelocityY = -9.5f;
    /// <summary> ジャンプ力の上限 </summary>
    float maxVelocityY = 3.5f;

    public PlayerState State
    {
        get { return state; }
        set { state = value; }
    }


    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }


    void Update()
    {
        ///Playerが操作可能だったら
        if (PlayerStatesManager.isOperation)
        {
            PlayerMove();

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
            freeLook.m_Lens.FieldOfView = Mathf.Lerp(50, 40, Time.deltaTime * 0.1f);
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

            if (Input.GetKey(KeyCode.LeftShift) && dir != Vector3.zero)
            {
                Vector3 velo = dir.normalized * m_runSpeed; // 入力した方向に移動する
                float velocityY = Mathf.Clamp(m_rb.velocity.y, minVelocityY, maxVelocityY);
                //freeLook.Priority = 9;
                //freeLook.m_Lens.FieldOfView = Mathf.Lerp(40, 50, Time.deltaTime * 0.1f);
                //velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
                velo.y = velocityY;
                m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
                state = PlayerState.Run;
            }
            else if (dir != Vector3.zero)
            {
                Vector3 velo = dir.normalized * m_walkSpeed; // 入力した方向に移動する
                float velocityY = Mathf.Clamp(m_rb.velocity.y, minVelocityY, maxVelocityY);
                //freeLook.Priority = 11;
                //freeLook.m_Lens.FieldOfView = Mathf.Lerp(50, 40, Time.deltaTime * 0.1f);
                //velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
                velo.y = velocityY;
                m_rb.velocity = velo;
                state = PlayerState.Walk;
            }
            //Vector3 velo = dir.normalized * m_movingSpeed; // 入力した方向に移動する
            
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
                //m_playerOperation = false;
                m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
                m_anim.SetBool("Jump", true);
                m_anim.SetBool("isGround", false);
                StartCoroutine(Jump());
            }
        }
        
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    void AttackMove()
    {
        ///CandyBeatの弱攻撃
        if (Input.GetButtonDown("Fire1") && animationEventScript.weaponStates == WeaponState.CandyBeat)
        {
            PlayerStatesManager.isOperation = false;
            m_anim.SetBool("Light", true);
            StartCoroutine(AttackMotionTimer());
        }
        ///CandyBeatの強攻撃
        if (Input.GetButtonDown("Fire2") && animationEventScript.weaponStates == WeaponState.CandyBeat)
        {
            PlayerStatesManager.isOperation = false;
            m_rb.AddForce(Vector3.up * 4, ForceMode.Impulse);
            m_anim.SetBool("Strong", true);
            StartCoroutine(AttackMotionTimer());
        }
        ///PopLauncherの射撃
        if (Input.GetButtonDown("Fire1") && animationEventScript.weaponStates == WeaponState.PopLauncher)
        {
            PlayerStatesManager.isOperation = false;
            m_anim.SetBool("Shoot", true);
            StartCoroutine(AttackMotionTimer());
            m_rb.velocity = new Vector3(0, m_rb.velocity.y, 0);
        }
    }

    /// <summary>
    /// 武器を切り替える
    /// </summary>
    public void WeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (animationEventScript.weaponStates == WeaponState.CandyBeat) return;

            EffectManager.PlayEffect(EffectType.ChangeWeapon, m_effectPos.position);
            animationEventScript.isChanged = false;
            animationEventScript.weaponStates = WeaponState.CandyBeat;

            if (m_shabadubiMode)
            {
                soundManager.PlaySeByName("Shabadubi");
            }
            else
            {
                soundManager.PlaySeByName("Change");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (animationEventScript.weaponStates == WeaponState.PopLauncher) return;
            
            EffectManager.PlayEffect(EffectType.ChangeWeapon, m_effectPos.position);
            animationEventScript.isChanged = false;
            animationEventScript.weaponStates = WeaponState.PopLauncher;

            if (m_shabadubiMode)
            {
                soundManager.PlaySeByName("Shabadubi");
            }
            else
            {
                soundManager.PlaySeByName("Change");
            }
        }
    }
    /// <summary>
    /// 硬直
    /// </summary>
    /// <returns> </returns>
    IEnumerator AttackMotionTimer()
    {
        m_anim.SetFloat("Move", 0);
        yield return new WaitForSeconds(m_waitTime);

        PlayerStatesManager.isOperation = true;
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.5f);

        m_anim.SetBool("isGround", false);
    }
}
