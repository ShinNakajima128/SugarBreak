using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField, Range(0f, 1f)] float m_masterVolume = 1.0f;
    [SerializeField, Range(0f, 1f)] float m_bgmVolume = 0.1f;
    [SerializeField, Range(0f, 1f)] float m_seVolume = 1.0f;
    [SerializeField, Range(0f, 1f)] float m_voiceVolume = 1.0f;
    [SerializeField] AudioClip[] m_bgms = null;
    [SerializeField] AudioClip[] m_ses = null;
    [SerializeField] AudioClip[] m_voices = null;
    [SerializeField] AudioSource m_bgmAudioSource = null;
    [SerializeField] AudioSource m_seAudioSource = null;
    [SerializeField] AudioSource m_voiceAudioSource = null;
    Dictionary<string, int> bgmIndex = new Dictionary<string, int>();
    Dictionary<string, int> seIndex = new Dictionary<string, int>();
    Dictionary<string, int> voiceIndex = new Dictionary<string, int>();
    /// <summary> マスター音量時のフラグ </summary>
    bool masterVolumeChange = false;
    /// <summary> BGM音量時のフラグ </summary>
    bool bgmVolumeChange = false;
    /// <summary> SE音量時のフラグ </summary>
    bool seVolumeChange = false;
    /// <summary> ボイス音量時のフラグ </summary>
    bool voiceVolumeChange = false;

    public float GetMasterVolume { get => m_masterVolume; }
    public float GetBgmVolume { get => m_bgmVolume; }
    public float GetSeVolume { get => m_seVolume; }
    public float GetVoiceVolume { get => m_voiceVolume; }

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < m_bgms.Length; i++)
        {
            bgmIndex.Add(m_bgms[i].name, i);
        }

        for (int i = 0; i < m_ses.Length; i++)
        {
            seIndex.Add(m_ses[i].name, i);
        }
    }

    private void Start()
    {
        if (Instance != null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (SceneManager.GetActiveScene().name == "Title")
            {
                PlayBgmByName("Title");
            }
            else if (SceneManager.GetActiveScene().name == "BakedPlain")
            {
                PlayBgmByName("BakedValley");
            }
            else if (SceneManager.GetActiveScene().name == "BakedValley")
            {
                PlayBgmByName("BakedValley");
            }
            else if (SceneManager.GetActiveScene().name == "LifeGame")
            {
                PlayBgmByName("LifeGame");
            }
            else if (SceneManager.GetActiveScene().name == "Bingo")
            {
                PlayBgmByName("Bingo");
            }
            else if (SceneManager.GetActiveScene().name == "Reversi")
            {
                PlayBgmByName("Reversi");
            }
        }  
    }

    /// <summary>
    /// Sceneが遷移した時にBGMを変更する
    /// </summary>
    /// <param name="nextScene">遷移後のScene</param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (Instance != null)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Title":
                    PlayBgmByName("Title");
                    break;
                case "BakedValley":
                    PlayBgmByName("BakedValley");
                    break;
                case "LifeGame":
                    PlayBgmByName("LifeGame");
                    break;
                    PlayBgmByName("Bingo");
                    break;
                case "Reversi":
                    PlayBgmByName("Reversi");
                    break;
            }
        } 
    }

    void Update()
    {
        VolumeChanger();
    }

    /// <summary>
    /// 各音量を変更する
    /// </summary>
    public void VolumeChanger()
    {
        if (m_bgmAudioSource && bgmVolumeChange || m_bgmAudioSource && masterVolumeChange)
        {
            m_bgmAudioSource.volume = m_bgmVolume * m_masterVolume;
            if (masterVolumeChange) masterVolumeChange = false;
            if (bgmVolumeChange) bgmVolumeChange = false;
        }

        if (m_seAudioSource && seVolumeChange || m_seAudioSource && masterVolumeChange)
        {
            m_seAudioSource.volume = m_seVolume * m_masterVolume;
            if (masterVolumeChange) masterVolumeChange = false;
            if (seVolumeChange) seVolumeChange = false;
        }

        if (m_voiceAudioSource && voiceVolumeChange || m_voiceAudioSource && masterVolumeChange)
        {
            m_voiceAudioSource.volume = m_voiceVolume * m_masterVolume;
            if (masterVolumeChange) masterVolumeChange = false;
            if (voiceVolumeChange) voiceVolumeChange = false;
        }
    }
    /// <summary>
    /// マスター音量を変更する
    /// </summary>
    /// <param name="masterValue"> 音量 </param>
    public void MasterVolChange(float masterValue)
    {
        m_masterVolume = masterValue;
        masterVolumeChange = true;
    }

    /// <summary>
    /// BGM音量を変更する
    /// </summary>
    /// <param name="bgmValue"> 音量 </param>
    public void BgmVolChange(float bgmValue)
    {
        m_bgmVolume = bgmValue;
        bgmVolumeChange = true;
    }

    /// <summary>
    /// SE音量を変更する
    /// </summary>
    /// <param name="seValue"> 音量 </param>
    public void SeVolChange(float seValue)
    {
        m_seVolume = seValue;
        seVolumeChange = true;
    }

    /// <summary>
    /// ボイス音量を変更する
    /// </summary>
    /// <param name="voiceValue"> 音量 </param>
    public void VoiceVolChange(float voiceValue)
    {
        m_voiceVolume = voiceValue;
        voiceVolumeChange = true;
    }

    int GetBgmIndex(string name)
    {
        if (bgmIndex.ContainsKey(name))
        {
            return bgmIndex[name];
        }
        else
        {
            return 0;
        }
    }

    int GetSeIndex(string name)
    {
        if (seIndex.ContainsKey(name))
        {
            return seIndex[name];
        }
        else
        {
            return 0;
        }
    }
    void PlayBgm(int index)
    {
        if (Instance != null)
        {
            index = Mathf.Clamp(index, 0, m_bgms.Length);

            m_bgmAudioSource.clip = m_bgms[index];
            m_bgmAudioSource.loop = true;
            m_bgmAudioSource.volume = m_bgmVolume * m_masterVolume;
            m_bgmAudioSource.Play();
        }
    }

    public void PlayBgmByName(string name)
    {
        PlayBgm(GetBgmIndex(name));
        Debug.Log("再生");
    }

    public void StopBgm()
    {
        m_bgmAudioSource.Stop();
        m_bgmAudioSource.clip = null;
    }

    void PlaySe(int index)
    {
        index = Mathf.Clamp(index, 0, m_ses.Length);

        m_seAudioSource.PlayOneShot(m_ses[index], m_seVolume * m_masterVolume);
    }

    public void PlaySeByName(string name)
    {
        PlaySe(GetSeIndex(name));
    }

    void StopSe()
    {
        m_seAudioSource.Stop();
        m_seAudioSource.clip = null;
    }
}