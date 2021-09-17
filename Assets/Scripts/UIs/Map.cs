using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Map : MonoBehaviour
{
    public static Map Instance;

    [SerializeField]
    GameObject m_mapUI;

    [SerializeField]
    GameObject m_mainPanelUI = default;

    [SerializeField]
    Volume m_volume = default;

    DepthOfField m_depthOfField;
    bool pauseFlag = false;

    public bool PauseFlag => pauseFlag;

    private void Awake()
    {
        Instance = this;
        m_volume.profile.TryGet(out m_depthOfField);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M) && MenuManager.Instance.MenuStates == MenuState.Close)
        {
            pauseFlag = !pauseFlag;

            if (pauseFlag)
            {
                m_mapUI.SetActive(true);
                m_mainPanelUI.SetActive(false);
                m_depthOfField.gaussianStart.value = 0;
                m_depthOfField.gaussianEnd.value = 0;
                PlayerStatesManager.Instance.OffOperation();
                Time.timeScale = 0;
            }
            else
            {
                m_mapUI.SetActive(false);
                m_mainPanelUI.SetActive(false);
                m_depthOfField.gaussianStart.value = 25.5f;
                m_depthOfField.gaussianEnd.value = 86;
                Time.timeScale = 1;
                PlayerStatesManager.Instance.OnOperation();
            }
        }
    }
}
