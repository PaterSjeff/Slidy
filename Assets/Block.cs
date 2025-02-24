using UnityEngine;

public class Block : MonoBehaviour, IInteraction
{
    public void Interact(GameObject target)
    {
        Debug.Log("Interacting with block: " + this.name);
    }
}
