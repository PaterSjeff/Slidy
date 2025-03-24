using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class Interactable : MonoBehaviour
{
    [SerializeField] private ObjectType _objectType = ObjectType.Null;
    [CanBeNull] public Action<Player> OnInteract;
    [SerializeField] private bool _isSolid = false;
    [SerializeField] [CanBeNull] private Damagable _damagable = null;
    [SerializeField] protected bool _isInteractable = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Interact(Player player)
    {
        OnInteract?.Invoke(player);

        Debug.Log("Interact with " + gameObject.name + "at " + gameObject.transform.position);
    }

    public Damagable GetDamagable() { return _damagable; }

    public bool GetIsSolid() { return _isSolid; }

    private void OnDestroy()
    {
        GameEvents.ObjectDestroyed(this);
    }

    private void Start()
    {
        if (_isInteractable) { GameEvents.ObjectSpawned(this); }
    }
    
    public bool GetIsInteractable() { return _isInteractable; }
    public ObjectType GetObjectType() { return _objectType; }
}
