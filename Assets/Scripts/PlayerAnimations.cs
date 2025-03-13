using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetDirection(Vector3 dir)
    {
        this.transform.localRotation = Quaternion.LookRotation(-dir);
    }
}
