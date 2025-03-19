using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<Interactable> OnObjectDestroyed;
    public static event Action<Interactable> OnObjectSpawned;
    public static event Action<Vector2Int> OnExitLevel;


    public static event Action<Player> OnPlayerDestroyed;

    public delegate Player SpawnPlayerDelegate(Player playerPrefab);
    public static event SpawnPlayerDelegate OnSpawnPlayer;
    public static event Action<Vector2Int> OnChangeLevel;

    public static void ObjectDestroyed(Interactable obj)
    {
        OnObjectDestroyed?.Invoke(obj);
    }

    public static void ObjectSpawned(Interactable obj)
    {
        OnObjectSpawned?.Invoke(obj);
    }
    
    public static void ExitLevel(Vector2Int direction)
    {
        OnExitLevel?.Invoke(direction);
    }

    public static void PlayerDestroyed(Player player)
    {
        OnPlayerDestroyed?.Invoke(player);
    }

    public static Player RequestPlayerSpawn(Player playerPrefab)
    {
        // This will call the delegate and return the spawned player.
        return OnSpawnPlayer?.Invoke(playerPrefab);
    }
}
