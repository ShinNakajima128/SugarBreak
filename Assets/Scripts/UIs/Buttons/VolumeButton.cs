using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SugarBreak;

public class VolumeButton : MonoBehaviour
{
    [SerializeField]
    Transform m_cursorTrans = default;

    public  void OnSelectButton()
    {
        if (m_cursorTrans != null)
        {
            MenuCursor.CursorMove(m_cursorTrans.position);
        }
    }
}
