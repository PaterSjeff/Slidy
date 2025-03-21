using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LevelDoor : Toggler
{
    [SerializeField] private Vector2Int doorDirection;
    [SerializeField] private Transform _entrancePoint;
    [SerializeField] private Transform _exitPoint;

    [SerializeField] private Block _leaveCollider;
    
    private Player _player;

    protected override void OnEnable()
    {
        base.OnEnable();
        _leaveCollider.OnInteract += ExitLevel;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _leaveCollider.OnInteract += ExitLevel;
    }
    public void Initialize()
    {

    }

    public void TriggerSequence(GameObject player)
    {
        Open();
        _usedOnce = true;
        var tweenSequence = player.transform.DOMove(_exitPoint.position, 0.5f);
        tweenSequence.OnComplete(Close);
    }
    
    public void SpawnPlayer(Player player)
    {
        player.gameObject.SetActive(true);
        TriggerSequence(player.gameObject);
    }

    private void ExitLevel(Player player)
    {
        //TODO Might need to make going out prettier.
        player.gameObject.SetActive(false);
        GameEvents.ExitLevel(doorDirection);
    }

    public Vector2Int GetDoorDirection()
    {
        return doorDirection;
    }
}
