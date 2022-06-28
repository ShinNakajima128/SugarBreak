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
        [Tooltip("カーソルの移動速度")]
        [SerializeField]
        float _moveSpeed = 0.1f;

        Image _cursorImage;
        public static MenuCursor Instance { get; private set; }
        public bool IsActive { get; set; } = false;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _cursorImage = GetComponent<Image>();
            OffCursor();
        }

        /// <summary>
        /// カーソルを移動する
        /// </summary>
        /// <param name="pos"> 移動先 </param>
        public static void CursorMove(Vector2 pos)
        {
            if (!Instance.IsActive)
            {
                OnCursor();
            }
            Instance.transform.DOMove(pos, Instance._moveSpeed);
        }

        /// <summary>
        /// カーソル表示をONにする
        /// </summary>
        public static void OnCursor()
        {
            Instance._cursorImage.enabled = true;
        }
        /// <summary>
        /// カーソル表示をOFFにする
        /// </summary>
        public static void OffCursor()
        {
            Instance._cursorImage.enabled = false;
        }
    }
}
