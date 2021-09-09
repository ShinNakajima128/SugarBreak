using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスが落としたチョコエッグから出現するアイテムの基底クラス
/// </summary>
public class BossDropItemBase : MonoBehaviour
{
    [SerializeField]
    protected Vector3 m_rotateValue = new Vector3(0, 0.01f, 0);

    void Start()
    {
        StartCoroutine(RotateObject());
    }

    IEnumerator RotateObject()
    {
        while (true)
        {
            transform.Rotate(m_rotateValue);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(gameObject.name + "を獲得した");
            GameManager.Instance.OnGameEnd();
            GameManager.Instance.IsBakeleValleyCleared = true;
            GameManager.Instance.IsStageUpdated = true;
            Destroy(gameObject);
        }
    }
}
