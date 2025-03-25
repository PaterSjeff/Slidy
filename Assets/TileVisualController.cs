using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.Serialization;

public class TileVisualController : MonoBehaviour
{
    public Vector2Int _gridPosition;
    public GameObject[] _visualVariants;
    private GameObject _currentVisual;
    public LayerMask _wallLayer;
    public float _checkDistance = 1.0f;
    public ObjectType objectType;

    [Serializable]
    private class Lookup : SerializableDictionary<TileType, GameObject>
    {
    }

    [SerializeField] private Lookup _tileVisualLookup;
    [SerializeField] Transform _visualHolder;

    [Button]
    public void UpdateVisual()
    {
        foreach (Transform child in _visualHolder)
        {
            child.gameObject.SetActive(false);
        }

        // Determine which visual variant to use based on neighbors
        var tileVariant = GetTileTypeBasedOnNeighbors();

        if (_tileVisualLookup.TryGetValue(tileVariant, out GameObject visualObject))
        {
            visualObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Tile Visual Lookup Failed because tile variant {tileVariant} was not found");
        }
    }

    private TileType GetTileTypeBasedOnNeighbors()
    {
        bool hasLeftNeighbor = HasNeighbor(Vector2Int.left);
        bool hasRightNeighbor = HasNeighbor(Vector2Int.right);
        bool hasTopNeighbor = HasNeighbor(Vector2Int.up);
        bool hasBottomNeighbor = HasNeighbor(Vector2Int.down);

        // Four connections
        if (hasLeftNeighbor && hasRightNeighbor && hasTopNeighbor && hasBottomNeighbor)
        {
            return TileType.Cross;
        }

        // Three connections (T splits)
        if (hasLeftNeighbor && hasRightNeighbor && hasTopNeighbor && !hasBottomNeighbor)
        {
            return TileType.TSplitSouth;
        }
        else if (hasLeftNeighbor && hasRightNeighbor && hasBottomNeighbor && !hasTopNeighbor)
        {
            return TileType.TSplitNorth;
        }
        else if (hasTopNeighbor && hasBottomNeighbor && hasLeftNeighbor && !hasRightNeighbor)
        {
            return TileType.TSplitEast;
        }
        else if (hasTopNeighbor && hasBottomNeighbor && hasRightNeighbor && !hasLeftNeighbor)
        {
            return TileType.TSplitWest;
        }

        // Two connections
        if (hasLeftNeighbor && hasRightNeighbor)
        {
            return TileType.StraightHorizontal;
        }
        else if (hasTopNeighbor && hasBottomNeighbor)
        {
            return TileType.StraightVertical;
        }
        else if (hasLeftNeighbor && hasTopNeighbor)
        {
            return TileType.CornerNW;
        }
        else if (hasTopNeighbor && hasRightNeighbor)
        {
            return TileType.CornerNE;
        }
        else if (hasRightNeighbor && hasBottomNeighbor)
        {
            return TileType.CornerSE;
        }
        else if (hasBottomNeighbor && hasLeftNeighbor)
        {
            return TileType.CornerSW;
        }

        // One connection
        if (hasLeftNeighbor)
        {
            return TileType.EndWest;
        }
        else if (hasRightNeighbor)
        {
            return TileType.EndEast;
        }
        else if (hasTopNeighbor)
        {
            return TileType.EndNorth;
        }
        else if (hasBottomNeighbor)
        {
            return TileType.EndSouth;
        }

        // No connections
        return TileType.Island;
    }

    private bool HasNeighbor(Vector2 direction)
    {
        RaycastHit hit;
        Vector3 rayDirection = new Vector3(direction.x, 0, direction.y);
        if (Physics.Raycast(transform.position, rayDirection, out hit, _checkDistance, _wallLayer))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileVisualController tileVisualController))
            {
                if (tileVisualController.objectType == objectType) return true;
            }
            else return false;
        }
        return false;
    }
}
