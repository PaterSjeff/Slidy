using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [CanBeNull] public Action<Player> OnInteract;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Interact(Player player)
    {
        OnInteract?.Invoke(player);
        
        Debug.Log("Interact with " + gameObject.name +"at " + gameObject.transform.position);
    }
}
