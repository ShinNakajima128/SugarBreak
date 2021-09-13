using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// カメラと対象との間の遮蔽物(Cover)を透明化します。
/// カメラに付加してください。
/// 透明にする遮蔽物は Renderer コンポーネントを付加している必要があります。
/// </summary>
public class CameraTransparent : MonoBehaviour
{
    /// <summary>
    /// 被写体
    /// </summary>
    [SerializeField]
    Transform m_player;

    /// <summary>
    /// 遮蔽物のレイヤー名のリスト。
    /// </summary>
    [SerializeField]
    List<string> m_coverLayerNameList;

    /// <summary>
    /// 遮蔽物とするレイヤーマスク。
    /// </summary>
    int m_layerMask;

    /// <summary>
    /// 今回の Update で検出された遮蔽物の Renderer コンポーネント。
    /// </summary>
    public List<Renderer> m_rendererHitsList = new List<Renderer>();

    /// <summary>
    /// 前回の Update で検出された遮蔽物の Renderer コンポーネント。
    /// 今回の Update で該当しない場合は、遮蔽物ではなくなったので Renderer コンポーネントを有効にする。
    /// </summary>
    public Renderer[] m_rendererHitsPrevs;


    // Use this for initialization
    void Start()
    {
        // 遮蔽物のレイヤーマスクを、レイヤー名のリストから合成する。
        m_layerMask = 0;
        foreach (string layerName in m_coverLayerNameList)
        {
            m_layerMask |= 1 << LayerMask.NameToLayer(layerName);
        }

    }


    // Update is called once per frame
    void Update()
    {
        // カメラと被写体を結ぶ ray を作成
        Vector3 difference = (m_player.transform.position - this.transform.position);
        Vector3 direction = difference.normalized;
        Ray ray = new Ray(this.transform.position, direction);

        // 前回の結果を退避してから、Raycast して今回の遮蔽物のリストを取得する
        RaycastHit[] hits = Physics.RaycastAll(ray, difference.magnitude, m_layerMask);


        m_rendererHitsPrevs = m_rendererHitsList.ToArray();
        m_rendererHitsList.Clear();
        // 遮蔽物は一時的にすべて描画機能を無効にする。
        foreach (RaycastHit hit in hits)
        {
            // 遮蔽物が被写体の場合は例外とする
            if (hit.collider.gameObject == m_player)
            {
                Debug.Log("プレイヤー");
                continue;
            }

            // 遮蔽物の Renderer コンポーネントを無効にする
            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
            Renderer ChildRenderer = hit.collider.gameObject.GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                m_rendererHitsList.Add(renderer);
                renderer.material.color = new Color(1, 1, 1, 0.3f);
                //renderer.enabled = false;
            }
            else if (renderer == null && ChildRenderer != null)
            {
                m_rendererHitsList.Add(ChildRenderer);
                ChildRenderer.material.color = new Color(1, 1, 1, 0.3f);
                //ChildRenderer.enabled = false;
            }
        }

        // 前回まで対象で、今回対象でなくなったものは、表示を元に戻す。
        foreach (Renderer renderer in m_rendererHitsPrevs.Except<Renderer>(m_rendererHitsList))
        {
            // 遮蔽物でなくなった Renderer コンポーネントを有効にする
            if (renderer != null)
            {
                renderer.material.color = new Color(1, 1, 1, 1);
                //renderer.enabled = true;
            }
        }
    }
}