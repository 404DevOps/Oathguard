using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCAbilities : MonoBehaviour
{
    public List<AbilityBase> Abilities;
    private AbilityExecutor _executor;

    private NPCEntity _npcEntity;

    public void Initialize(NPCEntity entity, List<AbilityBase> abilities)
    {
        _npcEntity = entity;
        _executor = entity.AbilityExecutor;

        Abilities = abilities;
        foreach (var ability in Abilities)
            ability.Initialize();
    }

    /// <summary>
    /// Should give the best Ability for the Enemy to use at this Moment, will get interesting with ranged.
    /// </summary>
    public AbilityBase GetAbility()
    {
        if (Abilities.Count > 1)
        {
            //todo, make weighting if multiple abilities
            return null;
        }
        else 
        {
            return Abilities.First();
        }

    }
}
