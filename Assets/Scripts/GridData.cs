using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GridData", menuName = "ScriptableObjects/GridData", order = 1)]
public class GridData : ScriptableObject
{
    
    private Dictionary<Vector2Int, GameObject> _solidData = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> _floorData = new Dictionary<Vector2Int, GameObject>();
    
    // Method to scan and populate the lists based on scene contents
    [Button("Scan for Interactions")]
    public void ScanForInteractions()
    {
        _solidData.Clear();
        _floorData.Clear();
        
        var colliders = GameObject.FindGameObjectsWithTag("Solid");
        foreach (var collider in colliders)
        {
            Vector2Int gridPosition = new Vector2Int((int)collider.transform.position.x, (int)collider.transform.position.z);
            
            var blockInteraction = collider.GetComponent<IInteraction>();
            if (blockInteraction != null && blockInteraction is Block)
            {
                _solidData.Add(gridPosition, collider.gameObject);
            }
            else
            {
                _solidData.Add(gridPosition, null);
            }
        }
        
        var tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var tile in tiles)
        {
            Vector2Int gridPosition = new Vector2Int((int)tile.transform.position.x, (int)tile.transform.position.z);
            _floorData.Add(gridPosition, tile);
        }

        // Optionally, log results to the console
        Debug.Log($"Found {_solidData.Count} solid objects, {_floorData.Count} floor interactions.");
    }

    public GameObject GetSolid(Vector2Int gridPosition)
    {
       return _solidData[gridPosition]; 
    }

    public GameObject GetFloor(Vector2Int gridPosition)
    {
        return _floorData[gridPosition];
    }
    
    
}
