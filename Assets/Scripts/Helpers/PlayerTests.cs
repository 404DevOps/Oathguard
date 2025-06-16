using System.Linq;
using UnityEngine;

public class PlayerTests : MonoBehaviour
{
    public bool IncludeInBuild;

    private void Start()
    {
        if (!Application.isEditor && !IncludeInBuild)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            var player = EntityManager.Instance.Player;
            var damage = new DamageContext(player, player) { FinalDamage = 5000 };
            player.Health.ApplyDamage(damage);   
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            var player = EntityManager.Instance.Player;

            bool isCrit = false;
            var dmg = Random.Range(1, 10);
            if(dmg > 7) isCrit = true;

            var damage = new DamageContext(player, player) { FinalDamage = dmg, IsCritical = isCrit };
            player.Health.ApplyDamage(damage);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            var npc = EntityManager.Instance.Entities.FirstOrDefault(e => e.Type != EntityType.Player);

            bool isCrit = false;
            var crit = Random.Range(1, 10);
            if (crit > 6) isCrit = true;

            var damage = new DamageContext(EntityManager.Instance.Player, npc) { FinalDamage = 1, IsCritical = isCrit };
            npc.Health.ApplyDamage(damage);
        }
    }
}
