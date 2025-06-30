using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveDisplayUI : Singleton<WaveDisplayUI>
{
    public GameObject WaveDisplay;
    private CanvasGroup canvasGroup;
    public TextMeshProUGUI WaveText;
    public float ShowDuration;
    public float FadeOutDuration;
    public float FadeInDuration;

    private void Start()
    {
        canvasGroup = WaveDisplay.GetComponent<CanvasGroup>();
    }
    internal void Show(int currentWave)
    {
        WaveText.text = "Wave " + (currentWave + 1).ToString();
        ToggleVisible(true);
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return WaitManager.Wait(ShowDuration);
        ToggleVisible(false);
    }

    private void ToggleVisible(bool show, bool instantFadeout = false)
    {
        //fade out
        if (!show & canvasGroup.alpha > 0)
        {
            if (instantFadeout)
                canvasGroup.alpha = 0;
            else
                StartCoroutine(FadePanel(FadeOutDuration, true));
        }
        else if (show && canvasGroup.alpha == 0) //fade in
            StartCoroutine(FadePanel(FadeInDuration));


    }

    private IEnumerator FadePanel(float duration, bool reverse = false)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var perc = elapsed / duration;
            canvasGroup.alpha = reverse ? 1-perc : perc;
            yield return null;
        }
    }
}
