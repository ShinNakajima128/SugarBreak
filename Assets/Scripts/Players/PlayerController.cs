using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int comboNum = 0;
    Coroutine combpCoroutine;

    public PlayerState State
    {
        get { return state; }
        set { state = value; }
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
                    velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
                    m_rb.velocity = velo;
                    state = PlayerState.Walk;
                }
                else
                {
                    Vector3 velo = dir.normalized * m_runSpeed; // 入力した方向に移動する
                    velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
                    m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
                    state = PlayerState.Run;
                }      
            }
        }
    }

    public void FallDown()
    {
        if (!IsGrounded())
        {
            m_rb.AddForce(Vector3.down * 50, ForceMode.Impulse);
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
                StartCoroutine(Jump());
                m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
                m_anim.SetBool("Jump", true);
                m_anim.SetBool("isGround", false);
            }
        }
        
    }

    /// <summary>
    /// キャンディビートの攻撃
    /// </summary>
    void AttackMove()
    {
        if (Input.GetButtonDown("Fire1") && animationEventScript.weaponStates == WeaponState.CandyBeat)
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
                m_rb.AddForce(Vector3.up * 3, ForceMode.Impulse);
                m_anim.SetBool("Strong", true);                     ///CandyBeatの強攻撃
                StartCoroutine(AttackMotionTimer(m_waitTime));
            }   
        }

        ///ポップランチャーの攻撃
        if (Input.GetButtonDown("Fire1") && animationEventScript.weaponStates == WeaponState.PopLauncher)
        {
            PlayerStatesManager.Instance.IsOperation = false;
            m_anim.SetBool("Shoot", true);
            StartCoroutine(AttackMotionTimer(m_waitTime));
            m_rb.velocity = new Vector3(0, m_rb.velocity.y, 0);
        }

        ///デュアルソーダの攻撃
        if (Input.GetButtonDown("Fire1") && animationEventScript.weaponStates == WeaponState.DualSoda)
        {
            if (comboNum == 3) return;

            m_rb.velocity = Vector3.zero;

            if(comboNum == 0)
            {
                m_anim.SetTrigger("SwordAttack1");
                comboNum = 1;
                combpCoroutine =  StartCoroutine(AttackMotionTimer(0.3f));
            }
            else if(comboNum == 1)
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
                SoundManager.Instance.PlaySeByName("Shabadubi");
            }
            else
            {
                SoundManager.Instance.PlaySeByName("Change");
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
                SoundManager.Instance.PlaySeByName("Shabadubi");
            }
            else
            {
                SoundManager.Instance.PlaySeByName("Change");
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (animationEventScript.weaponStates == WeaponState.DualSoda) return;

            EffectManager.PlayEffect(EffectType.ChangeWeapon, m_effectPos.position);
            animationEventScript.isChanged = false;
            animationEventScript.weaponStates = WeaponState.DualSoda;

            if (m_shabadubiMode)
            {
                SoundManager.Instance.PlaySeByName("Shabadubi");
            }
            else
            {
                SoundManager.Instance.PlaySeByName("Change");
            }
        }
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
        yield return new WaitForSeconds(0.5f);

        m_anim.SetBool("isGround", false);
    }
}
