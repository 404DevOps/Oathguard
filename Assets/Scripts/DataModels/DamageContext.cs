public class DamageContext
{
    public DamageContext(EntityBase origin, EntityBase target)
    {
        Origin = origin;
        Target = target;
    }
    public EntityBase Origin;
    public EntityBase Target;
    public float BaseDamage;
    public float FinalDamage;
    public bool IsCritical;
    public bool IsTrueDamage;
    public DamageType Type;

    public bool IsImmune;
    public DamageEffect SourceEffect;
}
