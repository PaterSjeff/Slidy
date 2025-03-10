using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] private Item _damagableItem;
    [SerializeField] private ItemTypes _damagableItemType;
    public bool TryToKill(Item item)
    {
        Debug.Log("Trying to kill " + item.GetItemType() + " with " + _damagableItemType);
        
        if (item.GetItemType() == _damagableItemType)
        {
            Destroy(this.gameObject);
            return true;
        }
        
        return false;
    }
}
