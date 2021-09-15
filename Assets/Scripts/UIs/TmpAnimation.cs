using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TmpAnimation : MonoBehaviour
{
    TextMeshProUGUI m_tmpText;

    void Start()
    {
        m_tmpText = GetComponent<TextMeshProUGUI>();
        m_tmpText.DOFade(0, 0);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(m_tmpText);

        for (int i = 0; i < tmproAnimator.textInfo.characterCount; i++)
        {
            tmproAnimator.DOFadeChar(i, 1, 1f).SetDelay(i * 0.1f);
        }

        yield return new WaitForSeconds(9.0f);

        for (int i = 0; i < tmproAnimator.textInfo.characterCount; i++)
        {
            tmproAnimator.DOFadeChar(i, 0, 1f).SetDelay(i * 0.1f);
        }
    }
}
