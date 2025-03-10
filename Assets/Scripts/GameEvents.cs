using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<Interactable> OnObjectDestroyed;
    
    public static void ObjectDestroyed(Interactable obj) 
    {
        OnObjectDestroyed?.Invoke(obj);
    }
}
