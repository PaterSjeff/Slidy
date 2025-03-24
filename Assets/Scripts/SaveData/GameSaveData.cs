using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class GameSaveData
{
    public Vector2Int currentRoomCoords;
    public Vector2Int currentDoorCoords;
    public int coins;
    public List<Item> inventory;
    public bool hasCompletedTutorial;
    // Add other variables as needed
}