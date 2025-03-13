using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private RoomManager _roomManager;

    [SerializeField] Player _playerPrefab;
    [SerializeField] Transform _playerSpawnPoint;

    [CanBeNull] private Player _player;
    private bool _isSpawningPlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _gridManager.Initialize();
        _roomManager.Initialize(_gridManager);
    }

    void Start()
    {
        if (!_player)
        {
            SpawnPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player && !_isSpawningPlayer)
        {
            _isSpawningPlayer = true;
            StartCoroutine(RespawnPlayer());
        }
    }

    private void SpawnPlayer()
    {
        _player = GameEvents.RequestPlayerSpawn(_playerPrefab);
        _player?.Initialize(_gridManager);
    }
    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(3);

        SpawnPlayer();
        _isSpawningPlayer = false;
    }
}
