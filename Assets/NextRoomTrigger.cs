using UnityEngine;

public class NextRoomTrigger : Block
{
    [SerializeField] private Vector2Int _exitDirection;
    
    public override void Interact(Player player)
    {
        Debug.Log("Next room trigger");
        
        if (_exitDirection == Vector2Int.zero)
        {
            Debug.LogError("You forgot to set an exit direction");
            return;
        }
        
        GameEvents.ExitLevel(_exitDirection);
    }
}
