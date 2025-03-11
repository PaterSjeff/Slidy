using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    
    [CanBeNull] public Action<Player> OnInteract;
    [SerializeField] private bool _isSolid = false;
    [SerializeField] [CanBeNull] private Damagable _damagable = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Interact(Player player)
    {
        OnInteract?.Invoke(player);
        
        Debug.Log("Interact with " + gameObject.name +"at " + gameObject.transform.position);
    }
    
    public Damagable GetDamagable() { return _damagable; }
    
    public bool GetIsSolid() { return _isSolid; }

    private void OnDestroy()
    {
        GameEvents.ObjectDestroyed(this);
    }
}
