using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class VisualUpdater : MonoBehaviour
{
    [SerializeField] private List<TileVisualController> tileVisualControllers;

    [Button]
    public void SetVisuals()
    {
        foreach (var tileVisual in tileVisualControllers)
        {
            tileVisual.UpdateVisual();
        }
    }
}
