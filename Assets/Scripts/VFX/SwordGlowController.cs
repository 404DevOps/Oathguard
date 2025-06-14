using UnityEngine;

public class SwordGlowController : MonoBehaviour
{
    [SerializeField] private Renderer swordRenderer;
    [SerializeField] private float glowIntensity = 20f;

    private Material glowMaterial;
    private Material defaultMaterial;

    void Awake()
    {
        if (swordRenderer == null)
            swordRenderer = GetComponent<Renderer>();

        // Create a unique material instance
        defaultMaterial = swordRenderer.material;
        
        glowMaterial = new Material(defaultMaterial);
        glowMaterial.EnableKeyword("_EMISSION");
    }

    public void EnableGlow(Color glowColor)
    {
        Color emissionColor = glowColor * glowIntensity;
        glowMaterial.SetColor("_EmissionColor", emissionColor);
        swordRenderer.material = glowMaterial;
    }

    public void DisableGlow()
    {
        swordRenderer.material = defaultMaterial;
    }
}
