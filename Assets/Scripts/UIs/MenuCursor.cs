using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SugarBreak
{
    /// <summary>
    /// メニューのカーソルの機能を持つクラス
    /// </summary>
    public class MenuCursor : MonoBehaviour
    {
        public static MenuCursor Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// カーソルを移動する
        /// </summary>
        /// <param name="pos"> 移動先 </param>
        public static void CursorMove(Vector2 pos)
        {
            Instance.transform.DOMove(pos, 0.25f);
        }
    }
}
