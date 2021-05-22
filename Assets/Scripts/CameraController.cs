using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour, ExampleControls.ICameraActions
{
    private ExampleControls inputCamera;
    [SerializeField] private Transform character;
    [SerializeField] private GameObject cameraObj;

    private Vector2 inputRightStick;
    private Vector3 lookPos;        //カメラが見る方向を格納
    private Vector3 followPos;      //CameraArmが追従する位置を格納
    private float horizontal;       //現在の水平角度
    private float vertical;        //現在の垂直角度


    private void Awake()
    {
        inputCamera = new ExampleControls();
        inputRightStick = Vector2.zero;
        inputCamera.Camera.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputCamera.Enable();
    }

    private void OnDisable()
    {
        inputCamera.Disable();
    }

    private void Update()
    {
        var position = character.position;
        lookPos = Vector3.Lerp(lookPos, position, 0.1f);    //カメラの見る位置を減速移動で計算
        followPos = Vector3.Lerp(followPos, position, 0.1f);    //追従する位置を減速移動で計算
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(inputRightStick.x) > 0.25f)
        {    //入力制限。いらないかも
            horizontal += inputRightStick.x * Time.deltaTime * 40;
        }

        if (Mathf.Abs(inputRightStick.y) > 0.25f)
        {    //入力制限。いらないかも
            vertical += inputRightStick.y * Time.deltaTime * 40;
            //回りすぎるとUnityでは上下反転してしまうので上限を設ける-98～98までなら設定可能だと思う
            vertical = Mathf.Clamp(vertical, -80, 80);
        }

        Transform myTransform;
        (myTransform = transform).rotation = Quaternion.Euler(0, horizontal, -vertical);
        myTransform.position = followPos;
        cameraObj.transform.LookAt(lookPos);
    }

    public void OnAxisRight(InputAction.CallbackContext context)
    {    //InputSystemから値を取得
        inputRightStick.x = context.ReadValue<Vector2>().x;
        inputRightStick.y = context.ReadValue<Vector2>().y;
    }

    //public void OnR_Stick_Button(InputAction.CallbackContext context)
    //{
    //    Debug.Log("ButtonLeftStick");        //カメラリセットの処理を書く予定でしたが、遅刻しているので割愛時間がアレば修正します
    //}

    public void OnLook(InputAction.CallbackContext context)
    {
        Debug.Log("ButtonLeftStick");
        //throw new System.NotImplementedException();
    }
}