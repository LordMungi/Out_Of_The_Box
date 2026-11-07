using UnityEngine;

public class DresserDrawer : MonoBehaviour
{
    private Animation _openAnimation;

    private void Start()
    {
        TryGetComponent(out _openAnimation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
            _openAnimation.Play();
    }
}
