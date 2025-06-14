
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/OathAura", fileName = "NewOathAura")]
public class OathAura : AuraBase
{
    public OathType OathType;

    [Header("Player Particles")]
    public GameObject AuraParticles;
    public Vector3 ParticleOffset;

    [Header("Sword Particles")]
    public Color SwordGlowColor;

    public override void OnApply(AuraInstance instance)
    {
        GameEvents.OnEntityDamaged.AddListener(OnEntityDamaged);
        if (AuraParticles != null)
        {
            var holder = instance.Target.AuraVisualsHolder.transform;
            var particles = Instantiate(AuraParticles, holder.position + ParticleOffset, Quaternion.identity, holder);
            instance.VisualInstances.Add(particles);
        }

        var glowController = instance.Target.Weapon.GetComponent<SwordGlowController>();
        glowController.EnableGlow(SwordGlowColor);
    }

    public override void OnExpire(AuraInstance instance)
    {
        GameEvents.OnEntityDamaged.RemoveListener(OnEntityDamaged);

        if (instance.VisualInstances != null)
        {
            foreach(var visualGO in instance.VisualInstances)
            Destroy(visualGO);
        }
        instance.VisualInstances.Clear();
        
        base.OnExpire(instance);
    }

    private void OnEntityDamaged(DamageEventArgs args)
    {
        Debug.Log("EntityDamaged registered in OathAura.");
    }
}

