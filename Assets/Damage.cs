using Unity.VisualScripting;
using UnityEngine;

public class Damage : InteractableListener
{   
    protected override void Interact(Player player)
    {
        DealDamage(player);
    }

    private void DealDamage(Player player)
    {
        player.GetHealth().Damage();
    }
}
