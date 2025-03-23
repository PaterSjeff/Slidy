using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LevelDoor : Toggler
{
    [SerializeField] private Vector2Int _doorDirection;
    [SerializeField] private Transform _entrancePoint;
    [SerializeField] private Transform _exitPoint;

    [SerializeField] private Block _leaveCollider;

    private Player _player;
    private bool _doorUsedOnce = false;

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

    public void TriggerSequence(Player player)
    {
        Open();
        _usedOnce = true;
        var tweenSequence = player.transform.DOMove(_exitPoint.position, 0.5f);
        tweenSequence.OnComplete(CompleteSequence(player));
    }

    private TweenCallback CompleteSequence(Player player)
    {
        Close();
        player.ActivatePlayer();

        return null;
    }

    public void SpawnPlayer(Player player)
    {
        player.transform.position = this.transform.position;
        player.gameObject.SetActive(true);
        TriggerSequence(player);
    }

    private void ExitLevel(Player player)
    {
        if (_doorUsedOnce && _useOnce) { return; }

        Debug.Log("Exit " + this.gameObject.transform.position);
        //TODO Might need to make going out prettier.
        player.DeactivatePlayer();
        player.gameObject.SetActive(false);
        GameEvents.ExitLevel(_doorDirection);
        _doorUsedOnce = true;
    }

    public Vector2Int GetDoorDirection()
    {
        return _doorDirection;
    }
}
