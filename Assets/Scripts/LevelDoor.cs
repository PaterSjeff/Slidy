using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LevelDoor : Toggler
{
    [SerializeField] private Transform _entrancePoint;
    [SerializeField] private Transform _exitPoint;

    private GameObject player;


    public void TriggerSequence(GameObject player)
    {
        Open();
        _usedOnce = true;
        var tweenSequence = player.transform.DOMove(_exitPoint.position, 0.5f);
        tweenSequence.OnComplete(Close);
    }
}
