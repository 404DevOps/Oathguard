public struct AuraExpiredEventArgs
{
    public AuraExpiredEventArgs(string entityId, string auraId)
    {
        EntityId = entityId;
        AuraId = auraId;
    }
    public string EntityId;
    public string AuraId;
}
