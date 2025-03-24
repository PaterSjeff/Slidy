using System;
using UnityEngine;
using ES3Internal;
using ES3Types;
public class SaveManager : MonoBehaviour
{
    private RoomManager _roomManager;
    private Player _player;
    
    public void Initialize(RoomManager roomManager, Player player)
    {
        _roomManager = roomManager;
        _player = player;

        GameEvents.OnExitLevel += OnExitLevel;
    }

    private void OnDisable()
    {
        GameEvents.OnExitLevel -= OnExitLevel;
    }

    private void OnExitLevel(Vector2Int exitDirection)
    {
        SaveGame();
    }

    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.currentRoomCoords = _roomManager.GetCurrentRoomCoords();
        //saveData.currentDoorCoords = _roomMananager.GetEnteredDoorCoords();
        saveData.coins = _player.GetInventory().GetCoins();
        //saveData.inventory = _player.GetInventory().GetItems();

        ES3.Save<GameSaveData>("PlayerSave", saveData);
    }

    public void LoadGame()
    {
        if (ES3.KeyExists("PlayerSave"))
        {
            GameSaveData saveData = ES3.Load<GameSaveData>("PlayerSave");
            _roomManager.SetCurrentRoomCoords(saveData.currentRoomCoords);
            _player.GetInventory().SetCoins(saveData.coins);
            //_player.GetInventory().SetItems(saveData.inventory);
            //TutorialManager.Instance.SetTutorialCompleted(saveData.hasCompletedTutorial);
        }
        else
        {
            // Initialize a new game
            _roomManager.SetCurrentRoomCoords(Vector2Int.zero);
            //Might need to reset?
            //TutorialManager.Instance.ResetTutorial();
        }
    }
}