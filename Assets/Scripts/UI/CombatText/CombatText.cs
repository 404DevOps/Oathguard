using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    public Action OnTextFinished;
    public EntityBase Entity;
    TextMeshProUGUI _text;
    public Vector3 StartPosition;

    public float MoveSpeed;
    public float Duration;
    public string Text;

    public float FontSizeUpFactor = 1.5f;
    public float FontSizeUpFactorCrit = 2f;//used to make number big before getting small
    public bool IsCrit = false;

    private float _timer;
    
    public Color Color;
    public float offsetX;

    public float upForce = 10;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>(); // Cache it once
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        var pos = new Vector3(StartPosition.x + offsetX, StartPosition.y, StartPosition.z);
        transform.position = pos;
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
        transform.position += new Vector3(0, upForce * MoveSpeed, 0) * Time.deltaTime;

        if (_text.alpha > 0)
        {
            _text.alpha -= Time.deltaTime / Duration;
        }

        if (_timer >= Duration)
            OnTextFinished?.Invoke();
    }
}