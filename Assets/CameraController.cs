using System;
using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera _currentCamera;
    private Transform _playerTransform;

    private RoomManager _roomManager;

    public void Initialize(RoomManager roomManager)
    {
        _roomManager = roomManager;

        _roomManager.OnRoomSwitched += OnRoomSwitched;

        var room = _roomManager.GetCurrentRoom();
        EnableRoomCamera(room);
    }

    void OnDisable()
    {
        _roomManager.OnRoomSwitched -= OnRoomSwitched;
    }

    public void OnRoomSwitched(Room newRoom)
    {
        EnableRoomCamera(newRoom);
    }

    private void EnableRoomCamera(Room room)
    {
        Debug.LogWarning($"EnableRoomCamera for {room.name}");
        // Disable current camera if exists
        if (_currentCamera != null)
        {
            _currentCamera.gameObject.SetActive(false);
        }

        // Find and enable new camera
        CinemachineCamera roomCamera = room.GetCamera();
        if (roomCamera != null)
        {
            roomCamera.gameObject.SetActive(true);
            _currentCamera = roomCamera;
        }
        else
        {
            Debug.LogWarning("No Cinemachine camera found in room " + room.Coords);
        }
    }
}
