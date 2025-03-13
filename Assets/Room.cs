using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Dictionary<Vector2Int, Interactable> _interactions = new Dictionary<Vector2Int, Interactable>();
    [SerializeField] private List<Interactable> _interactionList = new List<Interactable>();
    
    private GridManager _gridManager;
    
    public void Initialize(GridManager gridManager)
    {
        _gridManager = gridManager;
        
        CollectInteractions();
    }

    private void OnEnable()
    {
        GameEvents.OnObjectDestroyed += HandleObjectDestroyed;
        GameEvents.OnObjectSpawned += HandleObjectSpawned;
    }

    private void OnDisable()
    {
        GameEvents.OnObjectDestroyed -= HandleObjectDestroyed;
        GameEvents.OnObjectSpawned -= HandleObjectSpawned;
    }
    void HandleObjectDestroyed(Interactable obj)
    {
        var gridPosition = _gridManager.GetGridPosition(obj.transform.position);

        _interactions.Remove(gridPosition);
    }

    void HandleObjectSpawned(Interactable obj)
    {
        var gridPosition = _gridManager.GetGridPosition(obj.transform.position);
        _interactions.Add(gridPosition, obj);
        _interactionList.Add(obj);
    }

    private void CollectInteractions()
    {
        foreach (var interaction in _interactionList)
        {
            var gridPosition = _gridManager.GetGridPosition(interaction.transform.position);
            _interactions.Add(gridPosition, interaction);
        }
    }
}
