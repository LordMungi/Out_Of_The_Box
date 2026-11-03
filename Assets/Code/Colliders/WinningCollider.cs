using UnityEngine;

public class WinningCollider : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    private void OnTriggerEnter(Collider other)
    {
        menuManager.OpenWinScreen();
    }
}