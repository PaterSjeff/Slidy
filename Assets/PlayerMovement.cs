using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private GridManager _gridManager;
    private Player _player;
    private PlayerAnimations _playerAnimations;
    
    [SerializeField] private LayerMask _gridLayerMask;
    [SerializeField] private Transform _rayCastOrigin;

    [SerializeField] private float _moveSpeed = 5f;

    private bool _isMoving;
    public void Initialize(GridManager gridManager, Player player, PlayerAnimations playerAnimations)
    {
        _gridManager = gridManager;
        _player = player;
        _playerAnimations = playerAnimations;
    }

    void Update()
    {
        if (_isMoving) { return; }
        
        if (Input.GetKeyDown(KeyCode.W)) TryMove(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(Vector3.right);
        if (Input.GetKeyDown(KeyCode.S)) TryMove(Vector3.back);
        if (Input.GetKeyDown(KeyCode.A)) TryMove(Vector3.left);
    }

    private void TryMove(Vector3 direction)
    {
        // Create a ray starting from the player's current position going in the given direction.
        Ray ray = new Ray(_rayCastOrigin.position, direction);
        RaycastHit hit;
        
        _playerAnimations.SetDirection(direction);

        // Send the raycast. Make sure your grid colliders are on the layer specified by gridLayerMask.
        if (Physics.Raycast(ray, out hit, 100f, _gridLayerMask))
        {
            var startPoint = transform.position;
            var hitPoint = hit.point;
            hitPoint -= direction / 2; // Adjust for grid center

            // Get the world positions for the tiles to move through
            Vector3 moveStartPoint = startPoint;
            Vector3 moveEndPoint = _gridManager.GetWorldPosition(hitPoint);

            // Split movement into tiles and loop through each tile
            StartCoroutine(Move(moveStartPoint, moveEndPoint, direction));
        }
        else
        {
            Debug.Log("No grid detected in direction: " + direction);
        }
    }

    private IEnumerator Move(Vector3 startPoint, Vector3 endPoint, Vector3 direction)
    {
        _isMoving = true;
        
        Vector3 currentPosition = startPoint;
        float distance = Vector3.Distance(startPoint, endPoint);
        float step = _gridManager.GetCellSize();  // Use grid cell size for step
        int steps = Mathf.CeilToInt(distance / step);

        float durationPerStep = step / _moveSpeed;

        // Loop through each tile and tween to the next
        for (int i = 0; i < steps; i++)
        {
            Vector3 nextTilePosition = currentPosition + direction * step;

            // Tween to the next tile
            yield return transform.DOMove(nextTilePosition, durationPerStep).SetEase(Ease.Linear).WaitForCompletion();

            // Check for hazards on this tile
            bool hasInteraction = _gridManager.TryGetInteractable(nextTilePosition, out Interactable interactable);
            if (hasInteraction)
            {
                interactable.Interact(_player);
            }

            // Move to the next tile position
            currentPosition = nextTilePosition;
        }

        // After movement is complete, interact with the block
        FinishMovement(endPoint, direction);
    }

    private void FinishMovement(Vector3 endPoint, Vector3 direction)
    {
        var targetWallPosition = endPoint + direction;
        bool hasInteraction = _gridManager.TryGetInteractable(targetWallPosition, out Interactable interactable);
        if (hasInteraction)
        {
            var damagable = interactable.GetDamagable();
            if (damagable != null) { damagable.DealDamage(); }
            
            interactable?.Interact(_player);
        }
        
        _isMoving = false;
    }
}
