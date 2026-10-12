using System.Collections.Generic;
using UnityEngine;

public class ToyBox : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Dictionary<RobotPart.Type, RobotPart> _availableParts = new Dictionary<RobotPart.Type, RobotPart>();

    private void OnEnable()
    {
        EventBus.Subscribe<OnPartGrabbed>(OnPartGrabbed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPartGrabbed>(OnPartGrabbed);
    }

    private void OnPartGrabbed(in OnPartGrabbed context)
    {
        if (!_availableParts.ContainsKey(context.part.type))
            _availableParts.Add(context.part.type, context.part);
    }
}