using UnityEngine;

public class InteractableListener : MonoBehaviour
{
    [SerializeField] protected Interactable _interactable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void OnEnable()
    {
        _interactable.OnInteract += Interact;
    }

    protected void OnDisable()
    {
        _interactable.OnInteract -= Interact;
    }

    protected virtual void Interact(Player player)
    {
        
    }
}
