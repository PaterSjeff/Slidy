using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Health _health;
    [SerializeField] private PlayerAnimations _playerAnimations;
    [SerializeField] private Inventory _inventory;

    public enum PlayerState
    {
        Active,
        Inactive
    }
    private PlayerState _currentState = PlayerState.Inactive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(GridManager gridManager)
    {
        _playerMovement.Initialize(gridManager, this, _playerAnimations, _inventory);
        SetState(PlayerState.Inactive);
    }

    public Health GetHealth()
    {
        return _health;
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    private void SetState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Active:
                _playerMovement.enabled = true;
                break;

            case PlayerState.Inactive:
                _playerMovement.enabled = false;
                break;
        }
    }

    public void ActivatePlayer()
    {
        SetState(PlayerState.Active);
    }

    public void DeactivatePlayer()
    {
        SetState(PlayerState.Inactive);
    }
}
