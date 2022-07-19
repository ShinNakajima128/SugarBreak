//参考サイト:https://qiita.com/kirurobo/items/fb6b39a6097338f02eb4

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UniRx;
using UniRx.Triggers;

public class CursolController : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int x, int y);


    void Start()
    {
        
    }
}
