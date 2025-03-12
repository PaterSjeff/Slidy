using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private LevelDoor _levelDoor;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(_playerPrefab, transform);
        _levelDoor.TriggerSequence(player);
    }
}
