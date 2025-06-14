using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarUIHandler : MonoBehaviour
{
    private float _currentResource = 1;
    private float _maxResource = 1;

    [SerializeField] private Image _resourceBarImagePrimary;
    [SerializeField] private float _updateResourceSpeed;

    public void InitializeBar(float currentResource, float maxResource, Color color)
    {
        _resourceBarImagePrimary.color = color;
        SetNewValue(currentResource, maxResource, true);
    }
    public void SetNewValue(float currentResource, float maxResource, bool updateInstant = false)
    {
        _currentResource = currentResource;
        _maxResource = maxResource;
        UpdateBar(updateInstant);
    }

    private void UpdateBar(bool updateInstant = false)
    {
        var percentage = _currentResource / _maxResource;

        if (!updateInstant)
            StartCoroutine(SmoothChangeResource(percentage));
        else
            SetResourceInstant(percentage);
    }
    private void SetResourceInstant(float percentage)
    {
        _resourceBarImagePrimary.fillAmount = percentage;
    }
    private IEnumerator SmoothChangeResource(float healthPercentage)
    {
        float preChangePercentage = _resourceBarImagePrimary.fillAmount;
        float elapsed = 0f;
        while (elapsed < _updateResourceSpeed)
        {
            elapsed += Time.deltaTime;
            _resourceBarImagePrimary.fillAmount = Mathf.Lerp(preChangePercentage, healthPercentage, elapsed / _updateResourceSpeed);
            yield return null;
        }

        _resourceBarImagePrimary.fillAmount = healthPercentage;
    }
}