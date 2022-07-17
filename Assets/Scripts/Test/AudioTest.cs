using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AudioTest : MonoBehaviour
{
    [SerializeField]
    SEType _seType;

    [SerializeField]
    VOICEType _voiceType;

    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => AudioManager.PlaySE(_seType));
    }
}
