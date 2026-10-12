using System.Collections.Generic;
using UnityEngine;

public class ToyBox : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Dictionary<RobotPart.Type, RobotPart> _availableParts = new Dictionary<RobotPart.Type, RobotPart>();

    private void OnEnable()
    {
        EventBus.Subscribe<OnPartGrabbed>(OnPartGrabbed);
        EventBus.Subscribe<OnPartRequest>(OnPartRequest);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPartGrabbed>(OnPartGrabbed);
        EventBus.Unsubscribe<OnPartRequest>(OnPartRequest);
    }

    public void GrabPart(RobotPart part)
    {
        Debug.Log("Grab " + part.type + " part");
    }

    private void OnPartGrabbed(in OnPartGrabbed context)
    {
        if (!_availableParts.ContainsKey(context.part.type))
            _availableParts.Add(context.part.type, context.part);
    }

    private void OnPartRequest(in OnPartRequest context)
    {
        if (_availableParts.TryGetValue(context.type, out RobotPart output))
            EventBus.Raise<OnPartGrabRequest>(output);
    }
}