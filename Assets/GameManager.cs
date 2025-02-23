using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private PlayerMovement _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _gridManager.Initialize();
        _player.Initialize(_gridManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
