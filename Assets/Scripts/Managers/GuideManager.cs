using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GuideManager : MonoBehaviour
{
    [SerializeField]
    float _startAnimSpeed = 0.5f;

    [SerializeField]
    float _otherAnimSpeed = 0.25f;

    [SerializeField]
    float _nextGuideInterval = 1.5f;

    [SerializeField]
    Ease _startGuideAnimEaseType = Ease.OutBounce;

    [SerializeField]
    Ease _otherGuideAnimEasetype = Ease.Linear;

    [Header("UI_Objects")]
    [SerializeField]
    GameObject _guidePanel = default;
    
    [SerializeField]
    Image _grayoutImage = default;

    [SerializeField]
    Image[] _guideTextImages = default;

    [SerializeField]
    Image _operationFrame = default;

    [SerializeField]
    Sprite[] _grayoutSprites = default;

    [SerializeField]
    bool _debugMode = false;
    
    public static GuideManager Instance { get; private set; }
    
    void Awake()
    {
        Instance = this;
    }

    public void OnGuide()
    {
        StartCoroutine(StartGuide());
    }

    IEnumerator StartGuide()
    {
        if (_debugMode)
        {
            PlayerStatesManager.Instance.OnOperation();
            yield break;
        }

        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.IsPlayingMovie = true;
        Time.timeScale = 0;
        _guidePanel.SetActive(true);

        var waitInterval = new WaitForSecondsRealtime(_nextGuideInterval);
        var waitInput = new WaitUntil(() => Input.GetKeyDown(KeyCode.Joystick1Button1));

        

        //_guideTextImages[0].gameObject.transform.DOScale(1, _animSpeed)
        //                   .SetEase(_animEaseType)
        //                   .SetUpdate(true);

        _guideTextImages[0].gameObject.transform.DOLocalMove(Vector3.zero, _startAnimSpeed)
                           .SetEase(_startGuideAnimEaseType)
                           .SetUpdate(true);

        AudioManager.PlaySE(SEType.UI_ButtonSelect);
        yield return waitInterval;
        yield return waitInput;

        _grayoutImage.sprite = _grayoutSprites[0];

        _guideTextImages[0].enabled = false;
        _guideTextImages[1].gameObject.transform.DOScale(1, _otherAnimSpeed)
                           .SetEase(_otherGuideAnimEasetype)
                           .SetUpdate(true);

        _operationFrame.enabled = true;
        _operationFrame.DOFade(0f, 0.5f)
                       .SetLoops(-1, LoopType.Yoyo)
                       .SetUpdate(true);

        AudioManager.PlaySE(SEType.UI_ButtonSelect);
        yield return waitInterval;
        yield return waitInput;

        _operationFrame.enabled = false;
        _grayoutImage.sprite = _grayoutSprites[1];

        _guideTextImages[1].enabled = false;
        _guideTextImages[2].gameObject.transform.DOScale(1, _otherAnimSpeed)
                           .SetEase(_otherGuideAnimEasetype)
                           .SetUpdate(true);

        AudioManager.PlaySE(SEType.UI_ButtonSelect);
        yield return waitInterval;
        yield return waitInput;

        _grayoutImage.sprite = _grayoutSprites[2];

        _guideTextImages[2].enabled = false;
        _guideTextImages[3].gameObject.transform.DOScale(1, _otherAnimSpeed)
                           .SetEase(_otherGuideAnimEasetype)
                           .SetUpdate(true);

        AudioManager.PlaySE(SEType.UI_ButtonSelect);
        yield return waitInterval;
        yield return waitInput;

        //インプット情報をここでカット
        yield return null;

        GameManager.Instance.IsPlayingMovie = false;
        Time.timeScale = 1;
        _guidePanel.SetActive(false);
        PlayerStatesManager.Instance.OnOperation();
        AudioManager.PlaySE(SEType.UI_Cancel);
        Debug.Log("ガイド終了");
    }
}
