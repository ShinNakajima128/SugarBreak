using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// クリア演出のクラス
/// </summary>
public class ClearMovie : MonoBehaviour
{
    PlayableDirector director;
   
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        GameManager.GameEnd += PlayMovie;
    }
        
    /// <summary>
    /// クリアムービーを再生する
    /// </summary>
    void PlayMovie()
    {
        Debug.Log("クリア演出再生");
        director.Play();
    }
}
