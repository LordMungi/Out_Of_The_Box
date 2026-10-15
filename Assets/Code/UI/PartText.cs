using UnityEngine;
using UnityEngine.UI;

public class PartText : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    
    [SerializeField] private Text _leg;
    [SerializeField] private Text _arm;

    private void Start()
    {
        EventBus.Subscribe<OnPartGrabbed>(ChangePartText);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPartGrabbed>(ChangePartText);
    }

    private void ChangePartText(in OnPartGrabbed newStateEvent)
    {
        if (newStateEvent.part.type != RobotPart.Type.Shooter && newStateEvent.part.type != RobotPart.Type.Pusher)
            _leg.text = "Leg: " + newStateEvent.part;
        else
            _arm.text = "Arm: " + newStateEvent.part;
    }
}