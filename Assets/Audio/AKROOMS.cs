using UnityEngine;

public class SoloJugadorRoom : MonoBehaviour
{
    private AkRoom akRoom;

    private void Awake()
    {
        akRoom = GetComponent<AkRoom>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            akRoom.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            akRoom.enabled = false;
        }
    }
}