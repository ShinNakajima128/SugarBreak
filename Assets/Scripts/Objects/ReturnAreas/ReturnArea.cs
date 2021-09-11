using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnArea : MonoBehaviour
{
    public static ReturnArea Instance;

    GameObject player;

    public Vector3 ReturnPoint { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

        yield return new WaitForSeconds(1.0f);

        LoadSceneManager.Instance.FadeOut();
        PlayerStatesManager.Instance.OnOperation();
    }
}
