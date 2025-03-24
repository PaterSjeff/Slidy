using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    public Action<Room> OnRoomSwitched;

    //Current Room
    private Vector2Int _currentRoomCoords;
    private Room _currentRoom;

    //Room tracking
    private Dictionary<Vector2Int, Room> _allRooms = new Dictionary<Vector2Int, Room>();
    private Queue<Room> _inactiveRooms = new Queue<Room>();

    //maximum of rooms loaded
    private const int MAX_LOADED_DISTANCE = 1;

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

    public void Initialize(GridManager gridManager, Player player)
    {
        _gridManager = gridManager;
        PopulateAllRooms();
        //LoadInitialRoom(new Vector2Int(0, 0));
        ManageRoomLoading();

        _player = player;
    }

    public void SpawnNewPlayer(Player player)
    {
        _currentRoom.SpawnNewPlayer(player);
    }

    private void PopulateAllRooms()
    {
        _allRooms = new Dictionary<Vector2Int, Room>();
        foreach (var data in _roomData)
        {
            if (data._roomPrefab != null)
            {
                data._roomPrefab.transform.position = ConvertRoomCoordsToWorldCoords(data._roomCoordinate);
                data._roomPrefab.Initialize(_gridManager);
                data._roomPrefab.gameObject.SetActive(false); // Start as inactive

                _allRooms[data._roomCoordinate] = data._roomPrefab;
            }
        }

        foreach (var VARIABLE in _allRooms)
        {
            Debug.LogWarning($"Room {VARIABLE.Key} has room {VARIABLE.Value}");
        }
    }

    private void SwitchToRoom(Room newRoom, Vector2Int directionOffset)
    {
        newRoom.gameObject.SetActive(true);

        // Position player at opposite entry point
        Vector2Int oppositeDirection = -directionOffset;
        newRoom.SpawnPlayer(_player, oppositeDirection);

        // Deactivate current room
        /*if (_currentRoom != null)
        {
            _currentRoom.gameObject.SetActive(false);
            //_inactiveRooms.Enqueue(_currentRoom);
        }*/

        _currentRoom = newRoom;
        //_currentRoomCoords = newRoom.Coords;

        OnRoomSwitched?.Invoke(newRoom);
    }

    private void ManageRoomLoading()
    {
        // Deactivate rooms beyond our loading radius
        foreach (var roomEntry in _allRooms)
        {
            int distance = Mathf.Abs(roomEntry.Key.x - _currentRoomCoords.x) +
                           Mathf.Abs(roomEntry.Key.y - _currentRoomCoords.y);

            if (distance > MAX_LOADED_DISTANCE && roomEntry.Value != _currentRoom)
            {
                roomEntry.Value.gameObject.SetActive(false);
            }
            else if (roomEntry.Value != _currentRoom)
            {
                roomEntry.Value.gameObject.SetActive(true);
            }
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
        Debug.Log($"Loading room {newCoords}");

        foreach (var VARIABLE in _allRooms)
        {
            Debug.LogWarning($"Room {VARIABLE.Key} has already been loaded");
        }

        if (!_allRooms.TryGetValue(newCoords, out var newRoom))
        {
            Debug.LogWarning($"No room exists at {newCoords}");
            return;
        }

        _currentRoomCoords = newCoords;

        SwitchToRoom(newRoom, exitDirection);
        ManageRoomLoading();
    }

    private Vector3 ConvertRoomCoordsToWorldCoords(Vector2Int coords)
    {
        const float roomOffset = 13f;
        return new Vector3(coords.x * roomOffset, 0, coords.y * roomOffset);
    }

    public Room GetCurrentRoom()
    {
        return _currentRoom;
    }

    public Vector2Int GetCurrentRoomCoords()
    {
        return _currentRoomCoords;
    }

    public void SetCurrentRoomCoords(Vector2Int coords)
    {
        _currentRoomCoords = coords;
        LoadInitialRoom(coords);
        ManageRoomLoading();
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
