using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
    /// <summary>タイトル画面のBGM</summary>
    [SerializeField] AudioClip titleBGM;
    /// <summary>決定ボタンのSE</summary>
    [SerializeField] AudioClip enterSE;
    /// <summary>戻るボタンのSE</summary>
    [SerializeField] AudioClip returnSE;
    /// <summary>各ステージのBGM</summary>
    [SerializeField] AudioClip[] gameBGM;
    /// <summary>足音のSE</summary>
    [SerializeField] AudioClip footSE;
    /// <summary>ダメージのSE
    [SerializeField] AudioClip damageSE;
    /// <summary>ゲームオーバーのSE</summary>
    [SerializeField] AudioClip gameOver;
    /// <summary>trueでmute</summary>
    public bool m_mute = false;
    /// <summary>BGMのフェードアウトスピード</summary>
    [SerializeField] float fadeSpeed;
    [SerializeField] AudioSource audioSourceBGM;
    [SerializeField] AudioSource audioSourceSE;

    static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    Debug.LogError(typeof(SoundManager) + "Object noy faund");
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "Title")
        {
            PlayBgm(titleBGM);
        }
    }
    // シーンが切り替わったときにシーン名ごとにBGMを切り替える
    void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                PlayBgm(titleBGM);
                break;
            case "Stage1":
                PlayBgm(gameBGM[0]);
                break;
            case "Stage2":
                PlayBgm(gameBGM[1]);
                break;
            case "Stage3":
                PlayBgm(gameBGM[2]);
                break;
            case "Tutorial":
                PlayBgm(gameBGM[0]);
                break;
        }
    }

    void PlayBgm(AudioClip bgm)
    {
        audioSourceBGM.clip = bgm;
        audioSourceBGM.loop = true;
        audioSourceBGM.Play();
    }

    //ミュートを切り替える
    public void Mute()
    {
        audioSourceSE.mute = m_mute;
        audioSourceBGM.mute = m_mute;
    }

    //現在再生中のBGMをフェードアウト
    public IEnumerator FadeOutBGM()
    {
        while (audioSourceBGM.volume > 0)
        {
            audioSourceBGM.volume -= fadeSpeed;
            yield return new WaitForSeconds(0.017f);
        }
    }
    //SEを再生
    public void PlaySE(AudioClip SE)
    {
        audioSourceSE.pitch = 1;
        audioSourceSE.PlayOneShot(SE);
    }
}
