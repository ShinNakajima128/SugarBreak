using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonUIController : MonoBehaviour
{
    [Header("Axis/ボタン名")]
    [SerializeField]
    string _horizontalAxisString = "Horizontal";

    [SerializeField]
    string _verticalAxisString = "Vertical";

    [SerializeField]
    string _submitKeyString = "Submit";   //決定

    [SerializeField]
    List<GameObject> _panelList = default;

    [SerializeField]
    List<Button> _firstButtonList = new List<Button>();

    [SerializeField]
    bool _isActived = true;

    ///<summary> PCマウスカーソル表示制御用 </summary>
    private bool usingDesktopCursor = true;
    enum InputState
    {
        MouseKeyboard,
        Controler
    }

    InputState _InputState = InputState.MouseKeyboard;

    public static ButtonUIController Instance { get; private set; }
    public List<Button> FirstButtonList { get => _firstButtonList; set => _firstButtonList = value; }
    public bool IsActived { get => _isActived; set => _isActived = value; }

    private void Awake()
    {
        Instance = this;
    }

    void OnGUI()
    {
        //EventはOnGUIの中でのみ受け取れるのでここで実行
        DeviceChangeCheck();
    }

    public void OnCurrentPanelFirstButton(int index)
    {
        if (usingDesktopCursor)
        {
            return;
        }
        StartCoroutine(OnButtonSelect(index));
    }
    IEnumerator OnButtonSelect(int index)
    {
        if (!_isActived)
        {
            yield break;
        }

        yield return null;

        if (_panelList[index].activeSelf)
        {
            _firstButtonList[index].Select();
            yield break;
        }

        for (int i = 0; i < _panelList.Count; i++)
        {
            if (_panelList[i].activeSelf)
            {
                if (_firstButtonList[i] == null)
                {
                    yield break;
                }
                else if (_firstButtonList[i].gameObject.activeSelf)
                {
                    _firstButtonList[i].Select();
                    Debug.Log($"{ _firstButtonList[i].gameObject.name}を選択");
                    yield break;
                }
                else
                {
                    yield return null;

                    _firstButtonList[index].Select();
                }
            }
        }

    }
    int CurrentPanelCheck()
    {
        for (int i = 0; i < _panelList.Count; i++)
        {
            if (_panelList[i].activeSelf)
            {
                return i;
            }
        }
        return 0;
    }
    void DeviceChangeCheck()
    {
        switch (_InputState)
        {
            case InputState.MouseKeyboard:
                if (IsControlerInput())
                {
                    Cursor.visible = false;
                    _InputState = InputState.Controler;
                    usingDesktopCursor = false;
                    OnCurrentPanelFirstButton(CurrentPanelCheck());
                    Debug.Log("モード：コントローラー");
                }
                break;

            case InputState.Controler:
                if (IsMouseKeyboard())
                {
                    Cursor.visible = true;
                    _InputState = InputState.MouseKeyboard;
                    usingDesktopCursor = true;
                    EventSystem.current.firstSelectedGameObject = null;
                    EventSystem.current.SetSelectedGameObject(null);
                    Debug.Log("モード：マウス&キーボード");
                }
                break;
        }
    }
    /// <summary>
    /// マウス入力チェック
    /// </summary>
    /// <returns></returns>
    private bool IsMouseKeyboard()
    {
        // マウスのボタン
        if (Event.current.isMouse)
        {
            return true;
        }

        if (Mathf.Abs(Input.GetAxis("MouseX")) > Mathf.Epsilon ||
            Mathf.Abs(Input.GetAxis("MouseY")) > Mathf.Epsilon)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// キーボード＆コントローラーチェック
    /// </summary>
    /// <returns></returns>
    private bool IsControlerInput()
    {
        //ジョイスティック1のボタンをチェック
        // ※KeyCode.Joystick1Button19まである
        for (int i = 0; i < 19; i++)
        {
            KeyCode tKeyCode = KeyCode.Joystick1Button0 + i;
            if (Input.GetKey(tKeyCode))
            {
                return true;
            }
        }

        //ジョイスティック入力
        if (Mathf.Abs(Input.GetAxis(_horizontalAxisString)) > Mathf.Epsilon ||
            Mathf.Abs(Input.GetAxis(_verticalAxisString)) > Mathf.Epsilon)
        {
            return true;
        }
        return false;
    }
}
