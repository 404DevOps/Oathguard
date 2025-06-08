public struct AuraRefreshedEventArgs
{
    public AuraRefreshedEventArgs(string entityId, AuraInfo aura)
    {
        EntityId = entityId;
        Aura = aura;
    }
    public string EntityId;
    public AuraInfo Aura;
}
