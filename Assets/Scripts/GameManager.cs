using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] Player _playerPrefab;
    [SerializeField] Transform _playerSpawnPoint;
    
    [CanBeNull] private Player _player;
    private bool _isSpawningPlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _gridManager.Initialize();

        _player = SpawnPlayer();
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

    private Player SpawnPlayer()
    {
        var player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        player.Initialize(_gridManager);
        return player;
    }
    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(3);

        _player = SpawnPlayer();
        _isSpawningPlayer = false;
    }
}
