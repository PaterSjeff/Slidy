using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int _width = 10;
    private int _height = 10;
    private float _cellSize = 1;

    private Dictionary<Vector2Int, Block> _grid = new Dictionary<Vector2Int, Block>();
    [CanBeNull] private Block _nextInteractionBlock;
    
    [SerializeField] private Block _interactionBlock;
    
    [SerializeField] private GridData _gridData;
    
    public void Initialize()
    {
        var gridPosition = GetGridPosition(_interactionBlock.transform.position);
        _grid.Add(gridPosition, _interactionBlock);
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

    public void SetNextInteractionBlock(Vector3 position)
    {
        var gridPosition = GetGridPosition(position);
        _nextInteractionBlock = _grid[gridPosition];
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
