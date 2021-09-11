using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    Transform m_player = default;

    [SerializeField]
    CinemachineFreeLook m_freeLook = default;

    Vector3 m_initialPos;

    private void Start()
    {
        Instance = this;
        m_initialPos = m_freeLook.transform.localPosition;

        Debug.Log(m_initialPos);
    }

    public void CameraReset()
    {
        StartCoroutine(ResetPosition());
    }

    IEnumerator ResetPosition()
    {
        m_freeLook.Follow = null;
        Debug.Log("リセット開始");
        yield return null;
        m_freeLook.transform.localPosition = m_initialPos;
        Debug.Log("初期位置に戻す");
        yield return null;
        m_freeLook.Follow = m_player;
        Debug.Log("リセット完了");
    }
}
