using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform _playerTransform;
    private Dictionary<Vector2Int, Room> _roomPrefabs;
    private Vector2Int _currentRoomCoords;
    private Room _currentRoom;

    private GridManager _gridManager;

    public List<RoomData> _roomData = new List<RoomData>();

    private void OnEnable()
    {
        GameEvents.OnExitLevel += OnExitLevel;
    }

    private void OnDisable()
    {
        GameEvents.OnExitLevel -= OnExitLevel;
    }

    private void OnExitLevel(Vector2Int exitDirection)
    {
        LoadRoom(exitDirection);
    }

    public void Initialize(GridManager gridManager)
    {
        _gridManager = gridManager;

        //PopulateDictionary();

        LoadInitialRoom(new Vector2Int(0, 0));
    }

    public Player SpawnPlayer(Player player)
    {
        var temp = _currentRoom.SpawnPlayer(player);
        return temp;
    }
    
    private void PopulateDictionary()
    {
        _roomPrefabs = new Dictionary<Vector2Int, Room>();
        foreach (var data in _roomData)
        {
            if (data._roomPrefab != null)
            {
                _roomPrefabs[data._roomCoordinate] = data._roomPrefab;
            }
        }

        Debug.Log(_roomPrefabs.Count + " in the dictionary");
    }

    public void TeleportPlayer(Vector2 directionOffset)
    {
        Vector2Int nextRoomCoords = _currentRoomCoords;

        if (_currentRoom != null)
        {
            Destroy(_currentRoom);
        }

        if (_roomPrefabs.ContainsKey(nextRoomCoords))
        {
            Room newRoom = Instantiate(_roomPrefabs[nextRoomCoords]);
            _currentRoom = newRoom;
            _currentRoomCoords = nextRoomCoords;

            // Position player at the opposite entry point
            Vector2 oppositeDirection = -directionOffset;
            string entryPointName = GetEntryPointName(oppositeDirection);
            Transform entryPoint = _currentRoom.transform.Find(entryPointName);
            if (entryPoint != null)
            {
                _playerTransform.position = entryPoint.position;
            }
        }
        else
        {
            Debug.LogWarning($"No room exists at {nextRoomCoords}");
        }
    }

    private void LoadInitialRoom(Vector2Int coords)
    {
        Debug.Log($"Loading room {coords} and it {_roomPrefabs.Count}");
        _currentRoom = Instantiate(_roomPrefabs[coords]);
        _currentRoomCoords = coords;
        _currentRoom.Initialize(_gridManager);
        // Set initial player position, e.g., at a default entry point
        Transform entryPoint = _currentRoom.transform.Find("CenterEntry");
        if (entryPoint != null)
        {
            _playerTransform.position = entryPoint.position;
        }
    }

    private void LoadRoom(Vector2Int exitDirection)
    {
        _currentRoomCoords += exitDirection;
        _currentRoom = Instantiate(_roomPrefabs[_currentRoomCoords]);
        _currentRoom.transform.position = ConvertRoomCoordsToWorldCoords(_currentRoomCoords);
        _currentRoom.Initialize(_gridManager);
    }

    private Vector3 ConvertRoomCoordsToWorldCoords(Vector2Int coords)
    {
        var roomOffset = 13;
        return new Vector3(coords.x * roomOffset, 0, coords.y * roomOffset);
    }

    private string GetEntryPointName(Vector2 direction)
    {
        if (direction == new Vector2(0, 1)) return "SouthEntry";
        if (direction == new Vector2(0, -1)) return "NorthEntry";
        if (direction == new Vector2(1, 0)) return "WestEntry";
        if (direction == new Vector2(-1, 0)) return "EastEntry";
        return "CenterEntry"; // Fallback
    }

    [Button]
    private void LoadAllRooms()
    {
        foreach (var room in _roomData)
        {
            var temp = Instantiate(room._roomPrefab);
            temp.transform.position = ConvertRoomCoordsToWorldCoords(room._roomCoordinate);
            //_roomPrefabs.Add(room._roomCoordinate, temp);
        }
    }
}
