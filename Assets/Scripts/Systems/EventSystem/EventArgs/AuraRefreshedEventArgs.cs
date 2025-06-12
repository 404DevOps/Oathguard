public struct AuraRefreshedEventArgs
{
    public AuraRefreshedEventArgs(string entityId, AuraInstance aura)
    {
        EntityId = entityId;
        Aura = aura;
    }
    public string EntityId;
    public AuraInstance Aura;
}
