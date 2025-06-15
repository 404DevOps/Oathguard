using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    public EntityBase Entity;
    public Action OnTextFinished;

    private TextMeshProUGUI _text;

    public float MoveSpeed;
    public float Duration;
    public string Text;
    public bool IsCrit = false;    
    public Color Color;
    public float offsetX;
    private float _timer;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        transform.localScale = Vector3.one;
        _text.alpha = 1.0f;
        _text.richText = true;
        _text.text = Text;
        _text.color = Color;
        _timer = 0;
    }

    void LateUpdate()
    {
        _timer += Time.deltaTime;
        transform.position += new Vector3(0, MoveSpeed, 0) * Time.deltaTime;

        if (_text.alpha > 0)
        {
            _text.alpha -= Time.deltaTime / Duration;
        }

        if (_timer >= Duration)
            OnTextFinished?.Invoke();
    }
}