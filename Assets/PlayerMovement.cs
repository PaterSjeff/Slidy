using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private GridManager _gridManager;
    [SerializeField] private LayerMask _gridLayerMask;

    [SerializeField] private float _moveSpeed = 5f;

    public void Initialize(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right);
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.back);
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left);
    }

    private void Move(Vector3 direction)
    {
        // Create a ray starting from the player's current position going in the given direction.
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

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
            StartCoroutine(MoveThroughTiles(moveStartPoint, moveEndPoint, direction));
        }
        else
        {
            Debug.Log("No grid detected in direction: " + direction);
        }
    }

    private IEnumerator MoveThroughTiles(Vector3 startPoint, Vector3 endPoint, Vector3 direction)
    {
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
            Vector2Int gridPosition = _gridManager.GetGridPosition(nextTilePosition);

            // Move to the next tile position
            currentPosition = nextTilePosition;
        }

        // After movement is complete, interact with the block
        FinishMovement();
    }

    private void FinishMovement()
    {
        //_gridManager.InteractWithBlock();
    }
}
