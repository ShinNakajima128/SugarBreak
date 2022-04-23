using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioPlayer : MonoBehaviour
{
    [Tooltip("シナリオのデータ")]
    [SerializeField]
    ScenarioData m_scenarioData = default;

    [Tooltip("テキストの再生速度")]
    [SerializeField]
    float m_textSpeed = 2.0f;

    [Tooltip("次の文を表示するまでの時間")]
    [SerializeField]
    float m_flowTime = 2.0f;

    [Tooltip("Scene開始時にシナリオを再生するかどうか")]
    [SerializeField]
    bool m_playOnAwake = false;

    [Tooltip("シナリオを表示するText")]
    [SerializeField]
    TextMeshProUGUI m_scenarioText = default;

    /// <summary> 現在表示中のTextの指数 </summary>
    int _currentDialogIndex = 0;

    public static ScenarioPlayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (m_playOnAwake)
        {
            PlayScenario(m_scenarioData);
        }
    }

    IEnumerator PlayScenario(ScenarioData data)
    {
        while (_currentDialogIndex < data.DialogData.Length)
        {
            m_scenarioText.text = "";

            yield return new WaitForSeconds(m_flowTime);
            _currentDialogIndex++;
            BackgroundController.Instance.OnNextBackground();
        }
    }
}
