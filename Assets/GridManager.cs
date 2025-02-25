using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int _width = 10;
    private int _height = 10;
    private float _cellSize = 1;

    private Dictionary<Vector2Int, Interactable> _interactions = new Dictionary<Vector2Int, Interactable>();
    [SerializeField] private List<Interactable> _interactionList = new List<Interactable>();
    [CanBeNull] private Block _nextInteractionBlock;
    
    public void Initialize()
    {
        CollectInteractions();
    }
    
    private void CollectInteractions()
    {
        foreach (var interaction in _interactionList)
        {
            var gridPosition = GetGridPosition(interaction.transform.position);
            _interactions.Add(gridPosition, interaction);
        }
    }
    
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int((int)worldPosition.x, (int)worldPosition.z);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * _cellSize, gridPosition.y * _cellSize, 0);
    }

    public Vector3 GetWorldPosition(Vector3 worldPosition)
    {
        var gridPosition = GetGridPosition(worldPosition);
        return new Vector3(gridPosition.x * _cellSize, 0, gridPosition.y * _cellSize);
    }

    public bool TryGetInteractable(Vector3 position, out Interactable interactable)
    {
        var gridPosition = GetGridPosition(position);
        var hasInteraction = _interactions.TryGetValue(gridPosition, out interactable);

        return hasInteraction;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int x = 0; x <= _width; x++) // Draw lines vertically
        {
            var start = this.transform.position + new Vector3(x * _cellSize, 0, 0);
            var end = this.transform.position + new Vector3(x * _cellSize, 0, _height);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= _height; y++) // Draw lines horizontally
        {
            var start = this.transform.position + new Vector3(0, 0, y * _cellSize);
            var end = this.transform.position + new Vector3(_width, 0, y * _cellSize);
            Gizmos.DrawLine(start, end);
        }
    }

    public float GetCellSize() { return _cellSize; }

}
