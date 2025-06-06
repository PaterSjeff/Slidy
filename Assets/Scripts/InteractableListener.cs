using UnityEngine;

public class InteractableListener : MonoBehaviour
{
    
    [SerializeField] protected Interactable _interactable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void OnEnable()
    {
        _interactable.OnInteract += Interact;
    }

    protected virtual void OnDisable()
    {
        _interactable.OnInteract -= Interact;
    }

    protected virtual void Interact(Player player)
    {
        
    }

    public void SetInteractableController(Interactable interactable)
    {
        _interactable = interactable;
    }
}
