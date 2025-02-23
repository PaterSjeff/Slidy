using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private GridManager _gridManager;
    [SerializeField] private LayerMask _gridLayerMask;
    
    [SerializeField] private float _moveSpeed = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector3.forward);    
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3.right);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3.back);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3.left);
        }
    }

    private void Move(Vector3 direction)
    {
        // Create a ray starting from the player's current position going in the given direction.
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        // Send the raycast. Make sure your grid colliders are on the layer specified by gridLayerMask.
        if (Physics.Raycast(ray, out hit, 100f, _gridLayerMask))
        {
            // Get the collision point from the raycast hit.
            var hitPoint = hit.point;
            hitPoint -= direction/2;
            
            var interactPoint = hit.point + direction/2;
            _gridManager.SetNextInteractionBlock(interactPoint);

            var movePoint = _gridManager.GetWorldPosition(hitPoint);
            var distance = Vector3.Distance(transform.position, movePoint);
            var duration = distance/_moveSpeed;
            transform.DOMove(movePoint, duration).SetEase(Ease.Linear).OnComplete(FinishMovement);
        }
        else
        {
            Debug.Log("No grid detected in direction: " + direction);
        }
    }

    private void FinishMovement()
    {
        _gridManager.InteractWithBlock();
    }
}
