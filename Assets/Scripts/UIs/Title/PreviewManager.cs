using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PreviewManager : MonoBehaviour
{
    [SerializeField]
    float _waitTime = 15f;

    [SerializeField]
    GameObject _titleObject = default;

    [SerializeField]
    GameObject _moviePanel = default;

    VideoPlayer _player;

    void Start()
    {
        _player = GetComponent<VideoPlayer>();
        StartCoroutine(StartCount());
    }
    IEnumerator StartCount()
    {
        Debug.Log("カウント開始");
        float timer = 0;

        while (_waitTime >= timer)
        {
            if (InputDecision())
            {
                Debug.Log("カウントリセット");
                StartCoroutine(StartCount());
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        LoadSceneManager.Instance.FadeIn(callback: () =>
        {
            LoadSceneManager.Instance.FadeOut();
            _titleObject.SetActive(false);
            _moviePanel.SetActive(true);
            _player.Play();
            AudioManager.StopBGM();
            StartCoroutine(StopMovie());
            Debug.Log("ムービー再生");
        });
    }
    IEnumerator StopMovie()
    {
        while (!InputDecision())
        {
            yield return null;
        }

        LoadSceneManager.Instance.FadeIn(callback: () => 
        {
            LoadSceneManager.Instance.FadeOut();
            AudioManager.PlayBGM(BGMType.Title);
            _titleObject.SetActive(true);
            _player.Stop();
            _moviePanel.SetActive(false);
            StartCoroutine(StartCount());
        });
        Debug.Log("ムービーキャンセル");
    }
    bool InputDecision()
    {
        if (Input.anyKeyDown)
        {
            return true;
        }
        return false;
    }
}
