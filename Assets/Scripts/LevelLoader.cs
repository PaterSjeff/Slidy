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
    
    [SerializeField] private List<Interactable> _interactablesPrefabs = new List<Interactable>();

    private Dictionary<string, GameObject> prefabMap; // Maps type names to prefabs
    private Dictionary<ObjectType, Interactable> _objectMap;
    private Dictionary<string, GameObject> namedObjects; // Tracks objects with names for connections

    [SerializeField] private GameObject _levelContainer;

    [Button]
    private void StartLoadLevel()
    {
        // Initialize prefab mapping
        prefabMap = new Dictionary<string, GameObject>
        {
            { "Wall", _wallPrefab },
            { "Door", _doorPrefab },
            { "Lever", _leverPrefab },
            { "Spike Floor", _spikeFloorPrefab },
            { "Floor Button", _floorButtonPrefab },
            { "Enemy", _enemyPrefab },
            { "Coin", _coinPrefab },
            { "Sword", _swordPrefab }
        };

        foreach (var interactable in _interactablesPrefabs)
        {
            _objectMap.Add(interactable.GetObjectType(), interactable);
        }

        namedObjects = new Dictionary<string, GameObject>();
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
            if (!prefabMap.ContainsKey(obj.type))
            {
                Debug.LogWarning($"No prefab found for type: {obj.type}");
                continue;
            }

            Vector3 pos = new Vector3(obj.position[0] - 1, 0, obj.position[1] - 1);
            //GameObject instance = Instantiate(prefabMap[obj.type], pos, Quaternion.identity, _levelContainer.transform);
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabMap[obj.type], _levelContainer.transform);
            instance.transform.position = pos;
            //TODO need to do rotation for some things.

            // Store named objects for connections
            if (!string.IsNullOrEmpty(obj.name))
            {
                namedObjects[obj.name] = instance;
            }

            // Configure the object (e.g., set properties)
            ConfigureObject(instance, obj);
        }

        // Set up connections after all objects are spawned
        foreach (LevelObject obj in levelData.objects)
        {
            if (obj.connections != null && obj.connections.Length > 0)
            {
                GameObject source = namedObjects.ContainsKey(obj.name) ? namedObjects[obj.name] : null;
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
        switch (obj.type)
        {
            case "Wall":
                // No additional configuration needed
                break;

            case "Door":
                Toggler door = instance.AddComponent<Toggler>();
                door.SetState(obj.startingState);
                break;

            case "Lever":
                Toggler lever = instance.AddComponent<Toggler>();
                lever.SetState(obj.startingState);
                break;

            case "Spike Floor":
                Toggler spike = instance.AddComponent<Toggler>();
                spike.SetState(obj.startingState);
                break;

            case "Floor Button":
                Toggler button = instance.AddComponent<Toggler>();
                break;

            case "Enemy":
                Damage enemy = instance.AddComponent<Damage>();
                break;

            case "Coin":
                Pickup coin = instance.AddComponent<Pickup>();
                break;

            case "Sword":
                Pickup sword = instance.AddComponent<Pickup>();
                break;

            default:
                Debug.LogWarning($"Unhandled object type: {obj.type}");
                break;
        }
    }

    void SetupConnections(GameObject sourceObj, Connection[] connections, Dictionary<string, GameObject> namedObjects)
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
}

[System.Serializable]
public class Connection
{
    public string target;
    public string action;
}
