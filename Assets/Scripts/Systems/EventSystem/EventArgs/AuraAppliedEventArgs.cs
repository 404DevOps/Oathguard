public struct AuraAppliedEventArgs
{
    public AuraAppliedEventArgs(string entityId, AuraInfo aura)
    {
        EntityId = entityId;
        AuraInfo = aura;
    }
    public string EntityId;
    public AuraInfo AuraInfo;
}
