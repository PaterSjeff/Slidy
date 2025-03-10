using UnityEngine;

public class DamagerItem : Item
{
    public override void UseItem(Damagable damagable, out bool usedItem)
    {
        usedItem = damagable.TryToKill(this);
    }
}
