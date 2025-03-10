using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected ItemTypes _itemType;
    public virtual void UseItem(Damagable damagable, out bool usedItem)
    {
        Debug.Log("Item is used");
        usedItem = false;
    }
    
    public virtual ItemTypes GetItemType(){ return _itemType; }
}
