using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HurtFlash : MonoBehaviour
{
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private class RendererData
    {
        public Renderer renderer;
        public Material[] materials;
        public Color[] originalColors;
    }

    private List<RendererData> allRenderers = new();

    void Awake()
    {
        // Gather SkinnedMeshRenderers
        foreach (var smr in GetComponentsInChildren<SkinnedMeshRenderer>())
            CacheRenderer(smr);

        // Gather MeshRenderers
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            CacheRenderer(mr);
    }

    void CacheRenderer(Renderer renderer)
    {
        var mats = renderer.materials; // Instantiates materials if needed
        var originalColors = new Color[mats.Length];

        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i].HasProperty("_Color"))
                originalColors[i] = mats[i].color;
            else if (mats[i].HasProperty("_BaseColor"))
                originalColors[i] = mats[i].GetColor("_BaseColor");
            else
                originalColors[i] = Color.white;
        }

        allRenderers.Add(new RendererData
        {
            renderer = renderer,
            materials = mats,
            originalColors = originalColors
        });
    }

    public void FlashRed()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Set flash color
        foreach (var r in allRenderers)
        {
            for (int i = 0; i < r.materials.Length; i++)
            {
                var mat = r.materials[i];
                if (mat.HasProperty("_Color"))
                    mat.color = flashColor;
                else if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", flashColor);
            }
        }

        yield return new WaitForSeconds(flashDuration);

        // Revert colors
        foreach (var r in allRenderers)
        {
            for (int i = 0; i < r.materials.Length; i++)
            {
                var mat = r.materials[i];
                var original = r.originalColors[i];

                if (mat.HasProperty("_Color"))
                    mat.color = original;
                else if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", original);
            }
        }
    }
}
