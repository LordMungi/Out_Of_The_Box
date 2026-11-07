using UnityEngine;

public class DresserDrawer : MonoBehaviour
{
    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
            Debug.Log("Hit " + name + "!");
    }
}
