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
        Debug.Log(_currentItem.name + " is trying to deal with " + damagable.name);
        return damagable.TryToKill(_currentItem);
    }

    private void DropItem()
    {
        
    }
    
    public Item GetCurrentItem(){ return _currentItem; }
}
