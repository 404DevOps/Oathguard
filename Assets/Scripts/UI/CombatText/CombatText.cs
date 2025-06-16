using System;
using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    [Header("General")]
    public float MoveSpeed;
    public float Duration;
    public float TimeTillFadeOut;

    [Header("Regular")]
    public Vector3 StartOffset;
    private Vector3 _startScale = new Vector3(2, 2, 2);
    public float _regularScaleDuration = 0.05f;

    [Header("Crits")]
    public Vector3 StartOffsetCrit;
    private Vector3 _startScaleCrit = new Vector3(4, 4, 4);
    public float _critScaleDuration = 0.15f;

    public Action OnTextFinished;

    private EntityBase _entity;
    private TextMeshPro _tmpText;
    private float _timer;

    //text data
    private string _text;
    private bool _isCrit;
    private Vector3 _finalSize;

    //scaling
    private float _scaleDuration = 0.1f; // duration to shrink back to normal
    private bool _scalingDown = false;

    public void Setup(EntityBase entity, string text, Color color, bool isCrit)
    {
        if (_tmpText == null)
            _tmpText = GetComponentInChildren<TextMeshPro>();

        _entity = entity;
        _text = text;
        _isCrit = isCrit;
        color.a = 1;
        _tmpText.color = color;

        _tmpText.alpha = 1.0f;
        _timer = 0;

        _tmpText.text = _text;
        _tmpText.color = color;

        _scaleDuration = _isCrit ? _regularScaleDuration : _critScaleDuration;
        transform.localScale = _isCrit ? _startScale : _startScaleCrit;
        _finalSize = _isCrit ? new Vector3(1.5f, 1.5f, 1.5f) : Vector3.one;
        _scalingDown = true;

        Vector3 randomOffset = new Vector3(
                                        UnityEngine.Random.Range(-0.75f, 0.75f),
                                        UnityEngine.Random.Range(0f, 0.75f),
                                        0);

        transform.localPosition = _entity.CombatTextOffset + randomOffset;
    }

    void LateUpdate()
    {
        _timer += Time.deltaTime;

        if (!_isCrit) //dont move crit texts
            transform.position += new Vector3(0, MoveSpeed, 0) * Time.deltaTime;

        HandleScaling();
        HandleFadeout();

        if (_timer >= Duration)
            OnTextFinished?.Invoke();
    }

    private void HandleScaling()
    {
        if (_scalingDown)
        {
            float t = Mathf.Clamp01(_timer / _scaleDuration);
            transform.localScale = Vector3.Lerp(_startScale, _finalSize, t);

            if (t >= 1f)
                _scalingDown = false; // stop scaling down after done
        }
    }
    void HandleFadeout()
    {
        if (_timer < TimeTillFadeOut) return;

        if (_tmpText.alpha > 0)
        {
            var currentAlpha = _tmpText.alpha;
            float t = Mathf.Clamp01(_timer / Duration);
            _tmpText.alpha = Mathf.Lerp(currentAlpha, 0f, t);
        }
    }
}