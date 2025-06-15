public class DamageContext
{
    public EntityBase Origin;
    public EntityBase Target;
    public float BaseDamage;
    public float FinalDamage;
    public bool IsCritical;
    public bool IsTrueDamage;
    public DamageType Type;

    public bool IsImmune;

    public DamageEffect SourceEffect;
    public object SourceAbility;
}
