using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class OutlineActivator : MonoBehaviour
{
    ReactiveProperty<bool> _isInArea = new ReactiveProperty<bool>(false);
    public bool IsInArea => _isInArea.Value;
    public Action OnReactiveAction;
    public Action OffReactiveAction;

    void Start()
    {
        _isInArea.Subscribe((x) =>
        {
            if (x)
            {
                OnReactiveAction?.Invoke();
                //Debug.Log("リアクティブ:True");
            }
            else
            {
                OffReactiveAction?.Invoke();
                //Debug.Log("リアクティブ:false");
            }
        });
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("プレイヤーヒット");
            _isInArea.Value = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("プレイヤーが離れた");
            _isInArea.Value = false;
        }
    }
}
