using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] private Item _damagableItem;
    public bool TryToKill(Item item)
    {
        if (item == _damagableItem)
        {
            Destroy(this.gameObject);
            return true;
        }
        
        return false;
    }
}
