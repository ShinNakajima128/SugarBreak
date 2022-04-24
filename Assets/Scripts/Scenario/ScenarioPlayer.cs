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
    float m_textSpeed = 0.02f;

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
            StartCoroutine(PlayScenario(m_scenarioData));
        }
    }

    /// <summary>
    /// シナリオを再生する
    /// </summary>
    /// <param name="data"> シナリオのデータ </param>
    /// <returns></returns>
    IEnumerator PlayScenario(ScenarioData data)
    {
        while (_currentDialogIndex < data.DialogData.Length)
        {
            for (int i = 0; i < data.DialogData[_currentDialogIndex].Messages.Length; i++)
            {
                m_scenarioText.text = "";

                //テキストを一文字ずつ表示
                foreach (var t in data.DialogData[_currentDialogIndex].Messages[i])
                {
                    m_scenarioText.text += t;
                    yield return new WaitForSeconds(m_textSpeed);
                }

                yield return new WaitForSeconds(m_flowTime);    //次のメッセージに切り替わるまでの時間を待機
            }

            m_scenarioText.text = "";
            _currentDialogIndex++;
            bool isSwitched = false;
            
            BackgroundController.Instance.OnNextBackground(() => 
            {
                isSwitched = true;
            });

            yield return new WaitUntil(() => isSwitched);　//背景が切り替わるまで待機
        }
    }
}
