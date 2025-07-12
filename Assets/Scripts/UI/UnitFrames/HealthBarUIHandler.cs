using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIHandler : MonoBehaviour
{
    private float _currentHealth = 0;
    private float _maxHealth = 0;
    private float _currentShield = 0;

    [SerializeField] private Image _healthbarImagePrimary;
    [SerializeField] private Image _healthbarImageSecondary;
    [SerializeField] private Image _shieldBarImage;
    [SerializeField] private RectTransform _barContainer;

    [SerializeField] private float _updateHealthSpeed;

    Coroutine _coroutine;
    Coroutine _shieldCoroutine;
    public void InitializeBar(float currentHealth, float maxHealth)
    {
        SetNewHealth(currentHealth, maxHealth, true);
        SetNewShield(0, true);
    }
    public void SetNewShield(float currentShield, bool setInstant = false)
    {
        _currentShield = currentShield;
        UpdateShieldBar();// setInstant);
    }
    private void UpdateShieldBar()
    {
        float shieldPercent = Mathf.Clamp01(_currentShield / _maxHealth);

        _shieldBarImage.fillAmount = shieldPercent;
    }


    private IEnumerator SmoothShieldFill(float target)
    {
        float start = _shieldBarImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < _updateHealthSpeed)
        {
            elapsed += Time.deltaTime;
            _shieldBarImage.fillAmount = Mathf.Lerp(start, target, elapsed / _updateHealthSpeed);
            yield return null;
        }

        _shieldBarImage.fillAmount = target;
    }

    public void SetNewHealth(float currentHealth, float maxHealth, bool updateInstant = false)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
        UpdateBar(updateInstant);
    }

    private void UpdateBar(bool updateInstant = false)
    {
        var percentage = _currentHealth / _maxHealth;

        if (!updateInstant)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if(isActiveAndEnabled)
                _coroutine = StartCoroutine(SmoothChangeHealth(percentage));
        }

        else
            SetHealthInstant(percentage);
    }
    private void SetHealthInstant(float percentage)
    {
        _healthbarImagePrimary.fillAmount = percentage;
        _healthbarImageSecondary.fillAmount = percentage;
    }
    private IEnumerator SmoothChangeHealth(float healthPercentage)
    {
        float preChangePercentage = _healthbarImagePrimary.fillAmount;
        _healthbarImagePrimary.fillAmount = healthPercentage; //instant change primary
        float elapsed = 0f;
        while (elapsed < _updateHealthSpeed)
        {
            elapsed += Time.deltaTime;
            _healthbarImageSecondary.fillAmount = Mathf.Lerp(preChangePercentage, healthPercentage, elapsed / _updateHealthSpeed);
            yield return null;
        }

        _healthbarImageSecondary.fillAmount = healthPercentage;
    }
}