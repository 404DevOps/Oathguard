public struct AuraApplyResult
{
    public AuraApplyResult(EntityBase origin, EntityBase target, bool addedFresh, AuraInstance auraInfo)
    { 
        Origin = origin;
        Target = target;
        AddedFresh = addedFresh;
        AuraInfo = auraInfo;
    }
    public EntityBase Origin;
    public EntityBase Target;
    public AuraInstance AuraInfo;
    public bool AddedFresh;
}