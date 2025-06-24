public struct XPChangedEventArgs
{
    public PlayerEntity Entity;
    public float CurrentXP;
    public float MaxXP;
    public float XPGained;

    public XPChangedEventArgs(PlayerEntity entityBase, float currentxp, float maxxp, float xpgained)
    {
        Entity = entityBase;
        CurrentXP = currentxp;
        MaxXP = maxxp;
        XPGained = xpgained;
    }
}