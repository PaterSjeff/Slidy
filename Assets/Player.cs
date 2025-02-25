using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Health _health;
    [SerializeField] private PlayerAnimations _playerAnimations;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(GridManager gridManager)
    {
        _playerMovement.Initialize(gridManager, this, _playerAnimations);
    }

    public Health GetHealth()
    {
        return _health;
    }
}
