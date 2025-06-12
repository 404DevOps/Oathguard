
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/SealAura", fileName = "NewSealAura")]
public class SealAura : AuraBase
{
    public SealType SealType;
    public GameObject AuraParticles;
    public Vector3 ParticleOffset;

    public override void OnApply(AuraInstance instance)
    {
        GameEvents.OnEntityDamaged.AddListener(OnEntityDamaged);
        var holder = instance.Target.AuraVisualsHolder.transform;
        var particles = Instantiate(AuraParticles, holder.position + ParticleOffset, Quaternion.identity, holder);
        instance.VisualInstance = particles;
    }

    public override void OnExpire(AuraInstance instance)
    {
        GameEvents.OnEntityDamaged.RemoveListener(OnEntityDamaged);

        if (instance.VisualInstance != null)
            GameObject.Destroy(instance.VisualInstance);

        base.OnExpire(instance);
    }

    private void OnEntityDamaged(DamageEventArgs args)
    {
        Debug.Log("EntityDamaged registered in SealAura.");
    }
}

