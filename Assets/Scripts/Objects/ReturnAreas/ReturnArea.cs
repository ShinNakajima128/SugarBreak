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
            LoadSceneManager.Instance.FadeIn();
            StartCoroutine(Return());
        }
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(1.0f);

        player.transform.position = ReturnPoint;
        player.transform.rotation = ReturnRotation;

        yield return new WaitForSeconds(1.0f);
        LoadSceneManager.Instance.FadeOut();
        PlayerStatesManager.Instance.OnOperation();
        CameraManager.Instance.CameraReset();
    }
}
