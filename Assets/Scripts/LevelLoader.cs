using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.Serialization;

public class LevelLoader : MonoBehaviour
{
    [Header("Level File")]
    [SerializeField] private TextAsset _levelJsonFile; // Assign level.json in the Inspector

    [Header("Prefabs")]
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _doorPrefab;
    [SerializeField] private GameObject _leverPrefab;
    [SerializeField] private GameObject _spikeFloorPrefab;
    [SerializeField] private GameObject _floorButtonPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private List<InteractableData> _interactablesPrefabs = new List<InteractableData>();

    private Dictionary<ObjectType, GameObject> _prefabMap;
    private Dictionary<ObjectType, GameObject> namedObjects; // Tracks objects with names for connections

    [SerializeField] private GameObject _levelContainer;

    [Button]
    private void StartLoadLevel()
    {
        _prefabMap = new Dictionary<ObjectType, GameObject>();
        
        foreach (var interactable in _interactablesPrefabs)
        {
            _prefabMap.Add(interactable.objectType, interactable.interactableObject);
        }

        namedObjects = new Dictionary<ObjectType, GameObject>();
        LoadLevel();
    }

    void LoadLevel()
    {
        // Check if a level JSON file is assigned
        if (_levelJsonFile == null)
        {
            Debug.LogError("No level JSON file assigned!");
            return;
        }

        _levelContainer.transform.position = Vector3.zero;

        // Clear the namedObjects dictionary to avoid stale references
        namedObjects.Clear();

        // Deserialize JSON into LevelData
        LevelData levelData = JsonUtility.FromJson<LevelData>(_levelJsonFile.text);

        // Spawn level objects inside the level container
        foreach (LevelObject obj in levelData.objects)
        {
            if (!_prefabMap.ContainsKey(obj.objectType))
            {
                Debug.LogWarning($"No prefab found for type: {obj.type}");
                continue;
            }

            Vector3 pos = new Vector3(obj.position[0] - 1, 0, obj.position[1] - 1);
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(_prefabMap[obj.objectType], _levelContainer.transform);
            instance.transform.position = pos;
            //TODO need to do rotation for some things.

            // Store named objects for connections
            if (!string.IsNullOrEmpty(obj.name))
            {
                namedObjects[obj.objectType] = instance;
            }

            // Configure the object (e.g., set properties)
            ConfigureObject(instance, obj);
        }

        //Set up connections after all objects are spawned
        foreach (LevelObject obj in levelData.objects)
        {
            if (obj.connections != null && obj.connections.Length > 0)
            {
                GameObject source = namedObjects.ContainsKey(obj.objectType) ? namedObjects[obj.objectType] : null;
                if (source != null)
                {
                    SetupConnections(source, obj.connections, namedObjects);
                }
            }
        }
    }

    void ConfigureObject(GameObject instance, LevelObject obj)
    {
        // Add components and configure based on type
        switch (obj.objectType)
        {
            case ObjectType.Door:
                Toggler door = instance.GetComponent<Toggler>();
                door.SetState(obj.startingState);
                break;

            case ObjectType.BlockLever:
                Toggler lever = instance.GetComponent<Toggler>();
                lever.SetState(obj.startingState);
                break;

            case ObjectType.TileButton:
                break;

            case ObjectType.TileSpike:
                var spike = instance.GetComponent<Toggler>();
                spike.SetState(obj.startingState);
                break;

            case ObjectType.Enemy:
                break;

            case ObjectType.Coin:

                break;

            case ObjectType.Sword:

                break;

            default:
                Debug.LogWarning($"Unhandled object type: {obj.type}");
                break;
        }
    }

    void SetupConnections(GameObject sourceObj, Connection[] connections, Dictionary<ObjectType, GameObject> namedObjects)
    {
        foreach (Connection conn in connections)
        {
            // Find the target object in the namedObjects dictionary
            if (!namedObjects.TryGetValue(conn.target, out GameObject targetObj))
            {
                Debug.LogWarning($"Target object '{conn.target}' not found for connection.");
                continue;
            }

            // Handle LeverController as the source
            if (sourceObj.GetComponent<Interactable>() is { } lever)
            {
                if (targetObj.GetComponent<Toggler>() is { } door)
                {
                    door.SetInteractableController(lever);
                }
                else if (targetObj.GetComponent<Toggler>() is { } spike)
                {
                    spike.SetInteractableController(lever);
                }
                // Add more listener types here as needed
            }
            // Handle FloorButtonController as the source
            else if (sourceObj.GetComponent<Interactable>() is { } button)
            {
                if (targetObj.GetComponent<Toggler>() is { } spike)
                {
                    spike.SetInteractableController(button);
                }
                // Add more listener types here as needed
            }
            // Add more controller types here as needed
        }
    }
}

// Data structures for JSON deserialization
[System.Serializable]
public class LevelData
{
    public int[] playerStart;
    public LevelObject[] objects;
}

[System.Serializable]
public class LevelObject
{
    public string type;
    public int[] position;
    public string name;
    public bool startingState;
    public Connection[] connections;
    public ObjectType objectType;
}

[System.Serializable]
public class Connection
{
    public ObjectType target;
    public string action;
}
