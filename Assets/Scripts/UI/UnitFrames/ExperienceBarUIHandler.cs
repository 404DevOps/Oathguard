using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarUIHandler : MonoBehaviour
{
    private float _currentXP = 1;
    private float _maxXP = 1;

    [SerializeField] private Image _xpBarImagePrimary;
    [SerializeField] private float _updateXPSpeed;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _xpText;
    

    public void InitializeBar(float currentXP, float maxXP, int currentLevel)
    {
        StartCoroutine(SetNewValue(currentXP, maxXP, currentLevel, true));
    }
    public IEnumerator SetNewValue(float currentXP, float maxXP, int currentLevel, bool updateInstant = false)
    {
        _currentXP = currentXP;
        _maxXP = maxXP;
        yield return StartCoroutine(UpdateBar(updateInstant));
        UpdateTexts(currentXP, maxXP, currentLevel);
    }

    private void UpdateTexts(float currentXP, float maxXP, int currentLevel)
    {
        _levelText.text = currentLevel.ToString();
        _xpText.text = Mathf.CeilToInt(currentXP).ToString() + " / " + Mathf.CeilToInt(maxXP).ToString();
    }

    private IEnumerator UpdateBar(bool updateInstant = false)
    {
        var percentage = _currentXP / _maxXP;

        if (!updateInstant)
            yield return StartCoroutine(SmoothChangeXP(percentage));
        else
            SetXPInstant(percentage);
    }
    private void SetXPInstant(float percentage)
    {
        _xpBarImagePrimary.fillAmount = percentage;
    }
    private IEnumerator SmoothChangeXP(float xpPercentage)
    {
        float preChangePercentage = _xpBarImagePrimary.fillAmount;
        float elapsed = 0f;
        while (elapsed < _updateXPSpeed)
        {
            elapsed += Time.deltaTime;
            _xpBarImagePrimary.fillAmount = Mathf.Lerp(preChangePercentage, xpPercentage, elapsed / _updateXPSpeed);
            yield return null;
        }

        _xpBarImagePrimary.fillAmount = xpPercentage;
    }
}