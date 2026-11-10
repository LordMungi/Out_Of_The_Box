using UnityEngine;

public class DresserDrawer : MonoBehaviour
{
    private Animator _drawerAnimator;
    [SerializeField] private string triggerName;

    private void Start()
    {
        TryGetComponent(out _drawerAnimator);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
            _drawerAnimator?.SetTrigger(triggerName);
    }
}
