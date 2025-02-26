using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : InteractableListener
{
    [SerializeField] private int _pickupAmount;
    
    [SerializeField] [CanBeNull] private UnityEvent _onPickup;
    [SerializeField] [CanBeNull] private Item _pickupItem;
    protected override void Interact(Player player)
    {
        Inventory inventory = player.GetInventory();

        if (_pickupItem != null)
        {
            inventory.AddItem(_pickupItem);
        }
        else
        {
            inventory.AddCollectible(_pickupAmount);
            _onPickup?.Invoke(); 
        }
        
        Destroy(this.gameObject);
    }

    private void PickupItem()
    {
        
    }

    private void PickupCollectible()
    {
        
    }
}
