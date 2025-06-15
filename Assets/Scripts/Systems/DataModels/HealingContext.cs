public class HealingContext
{
    public EntityBase Origin;
    public EntityBase Target;

    public float BaseAmount;
    public float FinalAmount;

    public bool IsCritical;
    public bool IsTrueHealing;

    public object SourceAbility;
    public HealEffect SourceEffect;
}