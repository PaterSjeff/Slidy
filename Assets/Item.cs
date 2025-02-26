using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void UseItem()
    {
        Debug.Log("Item is used");
    }
}
