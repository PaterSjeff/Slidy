using UnityEngine;

public class Damagable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void DealDamage()
    {
        Destroy(this.gameObject);
    }
}
