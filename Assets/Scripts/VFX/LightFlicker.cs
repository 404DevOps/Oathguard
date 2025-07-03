using UnityEngine;


public class LightFlicker : MonoBehaviour
{
    Light Light;
    public float baseIntensity;
    public float flickerIntensity;
    private void Start()
    {
        Light = GetComponent<Light>();
    }
    private void Update()
    {
        baseIntensity = 4f;
        float flicker = Mathf.PerlinNoise(Time.time * 2f, 0f) * flickerIntensity;
        Light.intensity = baseIntensity + flicker;
    }
}

