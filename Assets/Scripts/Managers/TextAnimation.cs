using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_startTmp = default;
    Sequence seq;
    bool m_isPlaying = false;

    public static TextAnimation Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        OnAnim();
        m_isPlaying = true;
    }

    void OnDisable()
    {
        m_isPlaying = false;
    }

    /// <summary>
    /// アニメーションを開始する
    /// </summary>
    public void OnAnim()
    {
        StartCoroutine(LoopAnimation());
    }

    /// <summary>
    /// ループアニメーション
    /// </summary>
    IEnumerator LoopAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        
        while (m_isPlaying)
        {
            m_startTmp.DOFade(0, 0);
            DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(m_startTmp);

            for (int i = 0; i < tmproAnimator.textInfo.characterCount; i++)
            {
                tmproAnimator.DOScaleChar(i, 0.7f, 0);
                Vector3 currCharOffset = tmproAnimator.GetCharOffset(i);
                seq = DOTween.Sequence();

                seq.SetDelay(0.25f)
                   .Append(tmproAnimator.DOOffsetChar(i, currCharOffset + new Vector3(0, 30, 0), 0.4f).SetEase(Ease.OutFlash, 2))
                   .Join(tmproAnimator.DOFadeChar(i, 1, 0.4f))
                   .Join(tmproAnimator.DOScaleChar(i, 1, 0.4f).SetEase(Ease.OutBack))
                   .SetDelay(0.07f * i);
            }
            yield return new WaitForSeconds(3.0f);
        }
    }

    /// <summary>
    /// アニメーションを終了する
    /// </summary>
    public void FinishAnim()
    {
        if (seq != null) seq.Kill();
        m_isPlaying = false;
    }
}
