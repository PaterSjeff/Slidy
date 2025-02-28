using JetBrains.Annotations;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _coins;
    [SerializeField] [CanBeNull] private Item _currentItem;
    
    public void AddCollectible(int coins)
    {
        _coins += coins;
    }

    public void AddItem(Item item)
    {
        _currentItem = item;
    }

    public bool TryDealWithDamagable(Damagable damagable)
    {
        bool usedItem = false;
        _currentItem?.UseItem(damagable, out usedItem);
        return usedItem;
    }

    private void DropItem()
    {
        
    }
    
    public Item GetCurrentItem(){ return _currentItem; }
}
