using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //Current Room
    private Vector2Int _currentRoomCoords;
    private Room _currentRoom;

    //Room tracking
    private Dictionary<Vector2Int, Room> _allRooms = new Dictionary<Vector2Int, Room>();
    private Queue<Room> _inactiveRooms = new Queue<Room>();

    //maximum of rooms loaded
    private const int MAX_LOADED_ROOMS = 5;

    private GridManager _gridManager;

    public List<RoomData> _roomData = new List<RoomData>();

    private Player _player;

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
        PopulateAllRooms();
        LoadInitialRoom(new Vector2Int(0, 0));
    }

    public Player SpawnPlayer(Player player)
    {
        var temp = _currentRoom.SpawnPlayer(player);
        return temp;
    }

    private void PopulateAllRooms()
    {
        _allRooms = new Dictionary<Vector2Int, Room>();
        foreach (var data in _roomData)
        {
            if (data._roomPrefab != null)
            {
                Room newRoom = Instantiate(data._roomPrefab);
                newRoom.Coords = data._roomCoordinate;
                newRoom.transform.position = ConvertRoomCoordsToWorldCoords(data._roomCoordinate);
                newRoom.Initialize(_gridManager);
                newRoom.gameObject.SetActive(false); // Start as inactive

                _allRooms[data._roomCoordinate] = newRoom;
            }
        }
    }

    public void TeleportPlayer(Vector2 directionOffset)
    {
        Vector2Int nextRoomCoords = _currentRoomCoords + new Vector2Int((int)directionOffset.x, (int)directionOffset.y);

        if (!_allRooms.ContainsKey(nextRoomCoords))
        {
            Debug.LogWarning($"No room exists at {nextRoomCoords}");
            return;
        }

        Room targetRoom = _allRooms[nextRoomCoords];
        SwitchToRoom(targetRoom, directionOffset);
        ManageRoomLoading();
    }

    private void SwitchToRoom(Room newRoom, Vector2Int directionOffset)
    {
        if (!newRoom.isActiveAndEnabled)
        {
            newRoom.gameObject.SetActive(true);
        }

        // Position player at opposite entry point
        Vector2Int oppositeDirection = -directionOffset;
        newRoom.SpawnPlayer(_player, oppositeDirection);

        // Deactivate current room
        if (_currentRoom != null)
        {
            _currentRoom.gameObject.SetActive(false);
            _inactiveRooms.Enqueue(_currentRoom);
        }

        _currentRoom = newRoom;
        _currentRoomCoords = newRoom.Coords;
    }

    private void ManageRoomLoading()
    {
        // Unload rooms beyond our loading radius
        List<Vector2Int> roomsToUnload = new List<Vector2Int>();

        foreach (var roomEntry in _allRooms)
        {
            int distance = Mathf.Abs(roomEntry.Key.x - _currentRoomCoords.x) +
                           Mathf.Abs(roomEntry.Key.y - _currentRoomCoords.y);

            if (distance > 2 && roomEntry.Value != _currentRoom)
            {
                roomEntry.Value.gameObject.SetActive(false);
                _inactiveRooms.Enqueue(roomEntry.Value);
                roomsToUnload.Add(roomEntry.Key);
            }
        }

        foreach (var key in roomsToUnload)
        {
            _allRooms.Remove(key);
        }

        // Ensure we don't exceed our memory budget
        while (_inactiveRooms.Count > MAX_LOADED_ROOMS)
        {
            Room roomToDestroy = _inactiveRooms.Dequeue();
            Destroy(roomToDestroy.gameObject);
        }
    }

    private void LoadInitialRoom(Vector2Int coords)
    {
        if (!_allRooms.ContainsKey(coords))
        {
            Debug.LogError($"Initial room not found at {coords}");
            return;
        }

        _currentRoom = _allRooms[coords];
        _currentRoomCoords = coords;
        _currentRoom.gameObject.SetActive(true);

        // Set initial player position
        Transform entryPoint = _currentRoom.transform.Find("CenterEntry");
        if (entryPoint != null)
        {
            //_playerTransform.position = entryPoint.position;
        }
    }

    private void LoadRoom(Vector2Int exitDirection)
    {
        Vector2Int newCoords = _currentRoomCoords + exitDirection;

        if (!_allRooms.ContainsKey(newCoords))
        {
            Debug.LogWarning($"No room exists at {newCoords}");
            return;
        }

        Room newRoom = _allRooms[newCoords];
        SwitchToRoom(newRoom, exitDirection);
        ManageRoomLoading();
    }

    private Vector3 ConvertRoomCoordsToWorldCoords(Vector2Int coords)
    {
        const float roomOffset = 13f;
        return new Vector3(coords.x * roomOffset, 0, coords.y * roomOffset);
    }

    private string GetEntryPointName(Vector2 direction)
    {
        if (direction == Vector2.up) return "SouthEntry";
        if (direction == Vector2.down) return "NorthEntry";
        if (direction == Vector2.right) return "WestEntry";
        if (direction == Vector2.left) return "EastEntry";
        return "CenterEntry"; // Fallback
    }

    [Button]
    private void LoadAllRooms()
    {
        foreach (var roomData in _roomData)
        {
            Room room = Instantiate(roomData._roomPrefab);
            room.transform.position = ConvertRoomCoordsToWorldCoords(roomData._roomCoordinate);
            room.Coords = roomData._roomCoordinate;
        }
    }
}
