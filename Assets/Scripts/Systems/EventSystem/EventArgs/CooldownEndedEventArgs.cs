using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct CooldownEndedEventArgs
{
    public CooldownEndedEventArgs(string entityId, string abilityId)
    { 
        EntityId = entityId;
        AbilityId = abilityId;
    }
    public string EntityId;
    public string AbilityId;
}
