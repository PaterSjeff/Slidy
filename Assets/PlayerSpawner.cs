using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private LevelDoor _levelDoor;

    private void Start()
    {
        GameEvents.OnSpawnPlayer += SpawnPlayer;
    }
    private Player SpawnPlayer(Player playerPrefab)
    {
        Player player = Instantiate(playerPrefab, transform);
        _levelDoor.TriggerSequence(player.gameObject);

        return player;
    }
}
