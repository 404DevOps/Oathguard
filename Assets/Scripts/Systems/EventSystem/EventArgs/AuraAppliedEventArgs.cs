public struct AuraAppliedEventArgs
{
    public AuraAppliedEventArgs(string entityId, AuraInstance aura)
    {
        EntityId = entityId;
        AuraInstance = aura;
    }
    public string EntityId;
    public AuraInstance AuraInstance;
}