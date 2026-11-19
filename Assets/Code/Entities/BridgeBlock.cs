using UnityEngine;

public class BridgeBlock : MonoBehaviour
{
    private Animator _animator;
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void Start()
    {
        TryGetComponent(out _animator);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
     { 
            _animator?.SetTrigger("Hit");

            if (CompareTag("Shelf"))
                EventBus.Raise<OnBridgeFall>();
        }
    }
}
