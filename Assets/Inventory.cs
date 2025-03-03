using JetBrains.Annotations;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _coins;
    [SerializeField] [CanBeNull] private Item _currentItem;
    [SerializeField] Transform _itemPosition;

    private GameObject _itemGameObject;
    
    public void AddCollectible(int coins)
    {
        _coins += coins;
    }

    public void AddItem(Item item, GameObject itemGameObject)
    {
        _currentItem = item;
        _itemGameObject = Instantiate(itemGameObject, _itemPosition);
    }   
    public bool TryDealWithDamagable(Damagable damagable)
    {
        bool usedItem = false;
        _currentItem?.UseItem(damagable, out usedItem);
        if (usedItem)
        {
            _currentItem = null;
            Destroy(_itemGameObject);
        }
        return usedItem;
    }

    private void DropItem()
    {
        
    }
    
    public Item GetCurrentItem(){ return _currentItem; }
}
