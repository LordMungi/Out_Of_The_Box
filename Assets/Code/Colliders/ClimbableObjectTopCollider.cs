using UnityEngine;

public class ClimbableObjectTopCollider : MonoBehaviour
{
    public bool IsPlayerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            IsPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            IsPlayerInside = false;
    }
}