using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private SaveManager _saveManager;
    
    [SerializeField] Player _playerPrefab;
    [SerializeField] Transform _playerSpawnPoint;

    [SerializeField] private Player _player;
    void Awake()
    {
        _gridManager.Initialize();
        _roomManager.Initialize(_gridManager, _player);
        
        _saveManager.Initialize(_roomManager, _player);
        _saveManager.LoadGame();
        
        _cameraController.Initialize(_roomManager);
        
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (_player == null)
        {
            Debug.LogWarning("No player assigned to GameManager");
            return;
        }

        _player.gameObject.SetActive(false);
        _player.Initialize(_gridManager);
        _roomManager.SpawnNewPlayer(_player);
    }
}
