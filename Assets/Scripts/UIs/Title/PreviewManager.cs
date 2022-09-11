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
        _player.loopPointReached += FinishMovie;
        StartCoroutine(StartCount());
    }
    IEnumerator StartCount()
    {
        yield return null;

        Debug.Log("カウント開始");
        float timer = 0;

        while (_waitTime >= timer)
        {
            if (IsControlerInput())
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
        while (!IsControlerInput())
        {
            yield return null;
        }
        FinishMovie();
        //LoadSceneManager.Instance.FadeIn(callback: () => 
        //{
        //    LoadSceneManager.Instance.FadeOut();
        //    AudioManager.PlayBGM(BGMType.Title);
        //    _titleObject.SetActive(true);
        //    _player.Stop();
        //    _moviePanel.SetActive(false);
        //    StartCoroutine(StartCount());
        //});
        Debug.Log("ムービーキャンセル");
    }
   
    void FinishMovie(VideoPlayer vp = null)
    {
        LoadSceneManager.Instance.FadeIn(callback: () =>
        {
            LoadSceneManager.Instance.FadeOut();
            AudioManager.PlayBGM(BGMType.Title);
            _titleObject.SetActive(true);
            _player.Stop();
            _moviePanel.SetActive(false);
            StartCoroutine(StartCount());
        });
    }
    /// <summary>
    /// キーボード＆コントローラーチェック
    /// </summary>
    /// <returns></returns>
    private bool IsControlerInput()
    {
        //ジョイスティック1のボタンをチェック
        // ※KeyCode.Joystick1Button19まである
        for (int i = 0; i < 19; i++)
        {
            KeyCode tKeyCode = KeyCode.Joystick1Button0 + i;
            if (Input.GetKey(tKeyCode))
            {
                return true;
            }
        }

        //ジョイスティック入力
        if (Mathf.Abs(Input.GetAxis("UIHorizontal")) > Mathf.Epsilon ||
            Mathf.Abs(Input.GetAxis("UIVertical")) > Mathf.Epsilon)
        {
            return true;
        }
        return false;
    }
}
