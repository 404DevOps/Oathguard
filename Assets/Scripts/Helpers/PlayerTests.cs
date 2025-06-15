using UnityEngine;

public class PlayerTests : MonoBehaviour
{

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
            var damage = new DamageContext(player, player) { FinalDamage = 1 };
            player.Health.ApplyDamage(damage);
        }
    }
}
