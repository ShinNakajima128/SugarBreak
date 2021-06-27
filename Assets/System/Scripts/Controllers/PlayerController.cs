using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    [SerializeField] float m_waitTime = 1.0f;
    [SerializeField] AnimationEventScript animationEventScript = null; 
    /// <summary> プレイヤーが操作可能か否か </summary>
    [SerializeField] float m_isGroundedLength = 0.05f;
    public bool m_playerOperation = true;
    /// <summary> プレイヤーのRigidbody </summary>
    Rigidbody m_rb;
    Animator m_anim;
    float minVelocityY = -4.5f;
    float maxVelocityY = 3.5f;


    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (m_playerOperation)
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
                    m_anim.SetFloat("Move", velo.magnitude);
                }
            }
        }  
    }

    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        m_anim.SetBool("isGround", true);
        return isGrounded;
    }

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
        }
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);  // Slerp を使うのがポイント

            Vector3 velo = dir.normalized * m_movingSpeed; // 入力した方向に移動する
            float velocityY = Mathf.Clamp(m_rb.velocity.y, minVelocityY, maxVelocityY);
            //velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            velo.y = velocityY;
            m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }
    }

    void JumpMove()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                //m_playerOperation = false;
                m_rb.velocity = Vector3.zero;
                m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
                m_anim.SetBool("Jump", true);
                StartCoroutine(Jump());
            }
        }
        
    }

    void Rolling()
    { 
        //m_rb.velocity = Vector3.zero;
        m_rb.AddForce(transform.forward * 8, ForceMode.VelocityChange);
    }
    void AttackMove()
    {
        

        if (Input.GetButtonDown("Fire1") && animationEventScript.weaponStates == AnimationEventScript.WeaponState.CandyBeat)
        {
            m_playerOperation = false;
            m_anim.SetBool("Light", true);
            StartCoroutine(AttackMotionTimer());
        }

        if (Input.GetButtonDown("Fire1") && animationEventScript.weaponStates == AnimationEventScript.WeaponState.PopLauncher)
        {
            m_playerOperation = false;
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
            animationEventScript.isChanged = false;
            animationEventScript.weaponStates = AnimationEventScript.WeaponState.CandyBeat;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            animationEventScript.isChanged = false;
            animationEventScript.weaponStates = AnimationEventScript.WeaponState.PopLauncher;
        }
    }
    IEnumerator AttackMotionTimer()
    {
        m_anim.SetFloat("Move", 0);
        yield return new WaitForSeconds(m_waitTime);

        //if (m_anim.applyRootMotion) { m_anim.applyRootMotion = false; }
        m_playerOperation = true;
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.5f);

        m_anim.SetBool("isGround", false);
    }
}
