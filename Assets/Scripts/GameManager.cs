using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private CameraController _cameraController;

    [SerializeField] Player _playerPrefab;
    [SerializeField] Transform _playerSpawnPoint;

    [CanBeNull] private Player _player;
    void Awake()
    {
        _gridManager.Initialize();
        SpawnPlayer();
        _roomManager.Initialize(_gridManager, _player);
        _cameraController.Initialize(_roomManager);
    }

    private void SpawnPlayer()
    {
        _player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        _player?.gameObject.SetActive(false);
        _player?.Initialize(_gridManager);
        _roomManager.SpawnNewPlayer(_player);
    }
}
