using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private GrabberPart _grabberPart;

    private void Start()
    {
        if (_grabberPart == null)
        {
            Debug.Log("Hand GrabberPart is null");
            _grabberPart = GetComponent<GrabberPart>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            if (other.TryGetComponent(out RobotPart detectedPart))
            {
                _grabberPart.PartDetected(detectedPart);
            }
        }
        else
        {
            _grabberPart.WallDetected();
        }
    }
}