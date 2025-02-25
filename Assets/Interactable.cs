using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Interact()
    {
        Debug.Log("Interact with " + gameObject.name +"at " + gameObject.transform.position);
    }
}
