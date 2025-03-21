using UnityEngine;

[System.Serializable]
public class RoomData
{
    public Vector2Int _roomCoordinate;
    public Room _roomPrefab;
}

[System.Serializable]
public class DoorData
{
    public Vector2Int _doorCoordinate;
    public LevelDoor _door;
}
