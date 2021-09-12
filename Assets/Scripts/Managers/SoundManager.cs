using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [Header("マスター音量")]
    [SerializeField, Range(0f, 1f)] 
    float m_masterVolume = 1.0f;

    [Header("BGM音量")]
    [SerializeField, Range(0f, 1f)] 
    float m_bgmVolume = 0.3f;

    [Header("SE音量")]
    [SerializeField, Range(0f, 1f)] 
    float m_seVolume = 1.0f;

    [Header("ボイス音量")]
    [SerializeField, Range(0f, 1f)] 
    float m_voiceVolume = 1.0f;

    [Header("BGM")]
    [SerializeField] 
    AudioClip[] m_bgms = null;

    [Header("SE")]
    [SerializeField] 
    AudioClip[] m_ses = null;

    [Header("ボイス")]
    [SerializeField] 
    AudioClip[] m_voices = null;

    [Header("BGMのオーディオソース")]
    [SerializeField] 
    AudioSource m_bgmAudioSource = null;

    [Header("SEのオーディオソース")]
    [SerializeField] 
    AudioSource m_seAudioSource = null;

    [Header("ボイスのオーディオソース")]
    [SerializeField] 
    AudioSource m_voiceAudioSource = null;

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
    float currentVol;
    Coroutine coroutine;

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

        for (int i = 0; i < m_voices.Length; i++)
        {
            voiceIndex.Add(m_voices[i].name, i);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "Title")
        {
            PlayBgmByName("Title");
        }
        else if (SceneManager.GetActiveScene().name == "Base")
        {
            PlayBgmByName("Base");
        }
        else if (SceneManager.GetActiveScene().name == "BakedValley")
        {
            PlayBgmByName("BakeleValley1");
        }
        else if (SceneManager.GetActiveScene().name == "ModelTest")
        {
            PlayBgmByName("BakeleValley2");
        }

        currentVol = m_bgmAudioSource.volume;
    }

    /// <summary>
    /// Sceneが遷移した時にBGMを変更する
    /// </summary>
    /// <param name="nextScene">遷移後のScene</param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                SwitchBGM("Title");
                break;
            case "BakedValley":
                SwitchBGM("BakeleValley1");
                break;
            case "Base":
                SwitchBGM("Base");
                break;
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
            currentVol = m_bgmAudioSource.volume;

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

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="name"> BGMの名前 </param>
    public void PlayBgmByName(string name)
    {
        PlayBgm(GetBgmIndex(name));
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="name"> SEの名前 </param>
    public void PlaySeByName(string name)
    {
        PlaySe(GetSeIndex(name));
    }

    /// <summary>
    /// ボイスを再生する
    /// </summary>
    /// <param name="name"> ボイス音源名 </param>
    public void PlayVoiceByName(string name)
    {
        PlayVoice(GetVoiceIndex(name));
    }

    /// <summary>
    /// 再生中のBGMを停止する
    /// </summary>
    public void StopBgm()
    {
        m_bgmAudioSource.Stop();
        m_bgmAudioSource.clip = null;
    }
    /// <summary>
    /// 再生中のSEを停止する
    /// </summary>
    public void StopSe()
    {
        m_seAudioSource.Stop();
        m_seAudioSource.clip = null;
    }
     /// <summary>
     /// 再生中のボイスを停止する
     /// </summary>
    public void StopVoice()
    {
        m_voiceAudioSource.Stop();
        m_voiceAudioSource.clip = null;
    }

    /// <summary>
    /// BGMを滑らかに変更する
    /// </summary>
    /// <param name="afterBgm"></param>
    public void SwitchBGM(string afterBgm)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(SwitchingBgm(afterBgm));
        }
    }

    IEnumerator SwitchingBgm(string after)
    {

        while (m_bgmAudioSource.volume > 0)　//現在の音量を0にする
        {
            m_bgmAudioSource.volume -= 0.01f * 0.5f;
            yield return null;
        }

        var aft = GetBgmIndex(after);
        m_bgmAudioSource.clip = m_bgms[aft];　//BGMの入れ替え
        m_bgmAudioSource.Play();

        while (m_bgmAudioSource.volume < currentVol)　//音量を元に戻す
        {
            m_bgmAudioSource.volume += 0.01f * 0.5f;
            yield return null;
        }

        if (coroutine != null) coroutine = null;
        Debug.Log(currentVol);
    }

    int GetBgmIndex(string name)
    {
        if (bgmIndex.ContainsKey(name))
        {
            return bgmIndex[name];
        }
        else
        {
            Debug.Log("BGMが見つかりませんでした");
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
            Debug.Log("SEが見つかりませんでした");
            return 0;
        }
    }
    int GetVoiceIndex(string name)
    {
        if (voiceIndex.ContainsKey(name))
        {
            return voiceIndex[name];
        }
        else
        {
            Debug.Log("ボイスが見つかりませんでした");
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

    void PlaySe(int index)
    {
        index = Mathf.Clamp(index, 0, m_ses.Length);

        m_seAudioSource.PlayOneShot(m_ses[index], m_seVolume * m_masterVolume);
    }

    void PlayVoice(int index)
    {
        index = Mathf.Clamp(index, 0, m_voices.Length);

        m_voiceAudioSource.PlayOneShot(m_voices[index], m_voiceVolume * m_masterVolume);
    }
}