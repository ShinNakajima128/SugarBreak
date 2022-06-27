using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 拠点の各メニューリストの基底となるクラス
/// </summary>
public abstract class ListBase : MonoBehaviour
{
    [Tooltip("選択中のオブジェクト名を表示するText")]
    [SerializeField]
    TextMeshProUGUI _objectName = default;

    [Tooltip("オブジェクトの説明文を表示するText")]
    [SerializeField]
    TextMeshProUGUI _objectDescription = default;
}
