using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int Coords { get; set; }

    [SerializeField] private Transform _roomLayout;
    
    [SerializeField] private List<DoorData> _entryPoints = new List<DoorData>();
    private Dictionary<Vector2Int, LevelDoor> _entryPointsDic = new Dictionary<Vector2Int, LevelDoor>();
    
    private Dictionary<Vector2Int, Interactable> _interactions = new Dictionary<Vector2Int, Interactable>();
    [SerializeField] private List<Interactable> _interactionList = new List<Interactable>();

    private GridManager _gridManager;

    [SerializeField] private Transform _cameraTarget;

    public void Initialize(GridManager gridManager)
    {
        _gridManager = gridManager;
        
        GameEvents.OnObjectDestroyed += HandleObjectDestroyed;
        GameEvents.OnObjectSpawned += HandleObjectSpawned;

        SetInteractionsIntoDictionary();
    }

    private void SetInteractionsIntoDictionary()
    {
        foreach (var doorData in _entryPoints)
        {
            _entryPointsDic.Add(doorData._doorCoordinate, doorData._door);
        }
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

    public void SpawnPlayer(Player player, Vector2Int spawnCoords)
    {
        Debug.Log($"Spawning player {spawnCoords}");
        
        if (!_entryPointsDic.TryGetValue(spawnCoords, out var entryPoint))
        {
            Debug.LogError($"No entry point for {spawnCoords}");
            return;
        }
        
        entryPoint.SpawnPlayer(player);
    }

    public void SpawnNewPlayer(Player player)
    {
        //TODO we need to remember where we came from.
        SpawnPlayer(player, Vector2Int.up);
    }

    [Button]
    private void CollectInteractions()
    {
        _interactionList.Clear();
        _entryPoints.Clear();
        
        foreach (Transform child in _roomLayout)
        {
            if (!child.TryGetComponent<Interactable>(out var interactableObj)) { continue; }
            if (!interactableObj.GetIsInteractable()) { continue; }
            _interactionList.Add(interactableObj);

            if (interactableObj.TryGetComponent(out LevelDoor levelDoor))
            {
                var doorData = new DoorData();
                doorData._doorCoordinate = levelDoor.GetDoorDirection();
                doorData._door = levelDoor;
                _entryPoints.Add(doorData);
                Debug.Log($"{levelDoor.GetDoorDirection()} has been spawned");
            }
        }
    }

    public Transform GetGameraTarget()
    {
        return _cameraTarget;
    }
}
