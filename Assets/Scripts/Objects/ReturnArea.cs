using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnArea : MonoBehaviour
{
    [SerializeField]
    GameObject m_returnPoint = default;

    GameObject player;

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

        player.transform.position = m_returnPoint.transform.position;

        yield return new WaitForSeconds(1.0f);

        LoadSceneManager.Instance.FadeOut();
        PlayerStatesManager.Instance.OnOperation();
    }
}
