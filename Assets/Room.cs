using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int Coords { get; set; }
    
    private Dictionary<Vector2Int, Transform> _entryPoints = new Dictionary<Vector2Int, Transform>();
    
    
    private Dictionary<Vector2Int, Interactable> _interactions = new Dictionary<Vector2Int, Interactable>();
    [SerializeField] private List<Interactable> _interactionList = new List<Interactable>();

    private GridManager _gridManager;

    [ShowNonSerializedField] private PlayerSpawner _entrance;
    [ShowNonSerializedField] private Interactable _exit;

    public void Initialize(GridManager gridManager)
    {
        _gridManager = gridManager;
        
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

        if (!_interactions.TryAdd(gridPosition, obj)) { return; }
        _interactionList.Add(obj);
    }

    public Player SpawnPlayer(Player player, Vector2Int spawnCoords)
    {
        var temp = _entrance.SpawnPlayer(player);
        return temp;
    }

    [Button]
    private void CollectInteractions()
    {
        _interactionList.Clear();
        _entryPoints.Clear();
        
        foreach (Transform child in transform)
        {
            if (!child.TryGetComponent<Interactable>(out var interactableObj)) { continue; }
            if (!interactableObj.GetIsInteractable()) { continue; }
            _interactionList.Add(interactableObj);

            if (interactableObj.CompareTag("Entrance"))
            {
                _entrance = interactableObj.GetComponent<PlayerSpawner>();
            }
            if (interactableObj.CompareTag("Exit")) { _exit = interactableObj; }
        }
    }
}
