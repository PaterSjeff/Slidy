using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform playerTransform;
    private Dictionary<Vector2, GameObject> _roomPrefabs;
    private Vector2 _currentRoomCoords;
    private GameObject _currentRoom;

    public List<RoomData> roomData = new List<RoomData>();

    void Start()
    {
        
    }

    [Button]
    private void PopulateDictionary()
    {
        _roomPrefabs = new Dictionary<Vector2, GameObject>();
        foreach (var data in roomData)
        {
            if (data._roomPrefab != null)
            {
                _roomPrefabs[data._roomCoordinate] = data._roomPrefab;
            }
        }
    }

    public void TeleportPlayer(Vector2 directionOffset)
    {
        Vector2 nextRoomCoords = _currentRoomCoords + directionOffset;

        if (_currentRoom != null)
        {
            Destroy(_currentRoom);
        }

        if (_roomPrefabs.ContainsKey(nextRoomCoords))
        {
            GameObject newRoom = Instantiate(_roomPrefabs[nextRoomCoords]);
            _currentRoom = newRoom;
            _currentRoomCoords = nextRoomCoords;

            // Position player at the opposite entry point
            Vector2 oppositeDirection = -directionOffset;
            string entryPointName = GetEntryPointName(oppositeDirection);
            Transform entryPoint = _currentRoom.transform.Find(entryPointName);
            if (entryPoint != null)
            {
                playerTransform.position = entryPoint.position;
            }
        }
        else
        {
            Debug.LogWarning($"No room exists at {nextRoomCoords}");
        }
    }

    private void LoadInitialRoom(Vector2 coords)
    {
        _currentRoom = Instantiate(_roomPrefabs[coords]);
        _currentRoomCoords = coords;
        // Set initial player position, e.g., at a default entry point
        Transform entryPoint = _currentRoom.transform.Find("CenterEntry");
        if (entryPoint != null)
        {
            playerTransform.position = entryPoint.position;
        }
    }

    private string GetEntryPointName(Vector2 direction)
    {
        if (direction == new Vector2(0, 1)) return "SouthEntry";
        if (direction == new Vector2(0, -1)) return "NorthEntry";
        if (direction == new Vector2(1, 0)) return "WestEntry";
        if (direction == new Vector2(-1, 0)) return "EastEntry";
        return "CenterEntry"; // Fallback
    }
}
