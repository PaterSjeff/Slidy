using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : InteractableListener
{
    [SerializeField] private int _pickupAmount;
    [SerializeField] bool _isCollectible = true;
    [SerializeField] [CanBeNull] private UnityEvent _onPickup;
    [SerializeField] [CanBeNull] private Item _pickupItem;
    [SerializeField] [CanBeNull] private GameObject _pickupItemPrefab;
    [SerializeField] ItemTypes _itemType;
    protected override void Interact(Player player)
    {
        var inventory = player.GetInventory();

        if (!_isCollectible)
        {
            inventory.AddItem(_pickupItem, _pickupItemPrefab);
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
