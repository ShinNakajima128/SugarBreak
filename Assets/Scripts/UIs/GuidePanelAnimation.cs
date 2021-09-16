using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuidePanelAnimation : MonoBehaviour
{
    [SerializeField]
    GameObject m_rootPanel = default;
    
    private void OnEnable()
    {
        transform.localScale = Vector2.zero;
        OnAnim();
    }

    void OnAnim()
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.3f);
    }
}
