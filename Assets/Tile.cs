using UnityEngine;
[System.Serializable]
public class Tile
{
    public string name;
    public bool IsHazardous;  // Set this to true for lava, spikes, etc.

    public void Interact()
    {
        // Example of interaction; you can extend this with specific behavior
        Debug.Log("Interacting with tile: " + name);
    }
}
