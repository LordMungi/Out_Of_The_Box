using UnityEngine;

public class WoodenPlank : MonoBehaviour
{
    private Animator _animator;
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnEnable()
    {
        EventBus.Subscribe<OnBridgeFall>(OnBridgeFall);
    }
    private void Start()
    {
        TryGetComponent(out _animator);

        if (!_animator)
            Debug.Log("estoy nulo xd");
    }

    private void OnBridgeFall(in OnBridgeFall context)
    {
        _animator?.SetTrigger("Hit");
        Debug.Log("me cai");
    }
}
