using UnityEngine;

public class Tile : MonoBehaviour, IInteraction
{
    public void Interact(GameObject target)
    {
        // Example of interaction; you can extend this with specific behavior
        Debug.Log("Interacting with tile: " + this.name);
    }
}
