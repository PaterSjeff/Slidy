using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;


public class Toggler : InteractableListener
{
    [SerializeField] [CanBeNull] private Animation _animation;
    [SerializeField] [CanBeNull] private string _openChestClip;
    [SerializeField] [CanBeNull] private string _closeChestClip;
    
    [SerializeField] private bool _useOnce = false;
    
    private bool _usedOnce = false;
    private bool _isOpen = false;
    
    [SerializeField][CanBeNull] public UnityEvent _onOpen;
    [SerializeField][CanBeNull] public UnityEvent _onClose;
    
    protected override void Interact(Player player)
    {
        if (_useOnce)
        {
            if (_usedOnce) return;
            _usedOnce = true;
        }

        Toggle();
    }
    
    protected void Toggle()
    {
        if (_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    protected void Open()
    {
        _isOpen = true;
        _animation?.Play(_openChestClip);
        
        _onOpen?.Invoke();
    }

    protected void Close()
    {
        _isOpen = false;
        _animation?.Play(_closeChestClip);
        
        _onClose?.Invoke();
    }

    public void SetState(bool isOpen)
    {
        if (_isOpen == isOpen) Open();
        else Close();
    }
}
