using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ClearMovie : MonoBehaviour
{
    PlayableDirector director;
   
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        GameManager.GameEnd += PlayMovie;
    }

    void PlayMovie()
    {
        Debug.Log("クリア演出再生");
        director.Play();
    }
}
