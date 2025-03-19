using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LevelDoor : Toggler
{
    [SerializeField] private Vector2Int _entryPointCoord;
    [SerializeField] private Transform _entrancePoint;
    [SerializeField] private Transform _exitPoint;

    private GameObject player;

    public void Initialize()
    {
        GameEvents.OnSpawnPlayer += SpawnPlayer;
    }

    public void TriggerSequence(GameObject player)
    {
        Open();
        _usedOnce = true;
        var tweenSequence = player.transform.DOMove(_exitPoint.position, 0.5f);
        tweenSequence.OnComplete(Close);
    }

    public Player SpawnPlayer(Player playerPrefab)
    {
        Player player = Instantiate(playerPrefab);
        TriggerSequence(player.gameObject);

        return player;
    }

    public void GoToLevel()
    {
        GameEvents.ExitLevel(_entryPointCoord);
    }

    public Vector2Int GetEntryPointCoord()
    {
        return _entryPointCoord;
    }
}
