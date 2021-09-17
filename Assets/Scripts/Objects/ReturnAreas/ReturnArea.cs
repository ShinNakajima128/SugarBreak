using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触れたら現在の復帰場所に画面を暗転しながら移動させるクラス
/// </summary>
public class ReturnArea : MonoBehaviour
{
    public static ReturnArea Instance;

    GameObject player;

    public Vector3 ReturnPoint { get; set; }

    public Quaternion ReturnRotation { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ReturnPoint = player.transform.position;
        ReturnRotation = player.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatesManager.Instance.OffOperation();
            LoadSceneManager.Instance.FadeIn(LoadSceneManager.Instance.Masks[3]);
            ReturnComebackPoint();
            SoundManager.Instance.PlayVoiceByName("univ1093");
        }
    }

    public void ReturnComebackPoint()
    {
        StartCoroutine(Return());
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(1.0f);

        player.transform.position = ReturnPoint;
        player.transform.rotation = ReturnRotation;

        yield return new WaitForSeconds(1.0f);
        
        if (LoadSceneManager.Instance.LoadAnim.activeSelf) LoadSceneManager.Instance.LoadAnim.SetActive(false);
        LoadSceneManager.Instance.FadeOut(LoadSceneManager.Instance.Masks[4]);
        PlayerStatesManager.Instance.OnOperation();
        CameraManager.Instance.CameraReset();
        if (UIManager.Instance.BossUI.activeSelf) UIManager.Instance.BossUI.SetActive(false);
    }
}
