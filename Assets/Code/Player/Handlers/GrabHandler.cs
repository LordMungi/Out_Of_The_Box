using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Transform _worldParent;
    private Transform _handParent;

    private static RobotPart _grabbedItem;

    private static List<RobotPart> _nearbyItems = new List<RobotPart>();

    public GrabHandler(ref PlayerParents parents)
    {
        _worldParent = parents.worldParent;
        _handParent = parents.handParent;
    }

    public override void Enable()
    {
        base.Enable();
        EventBus.Subscribe<OnNearbyItemEntered>(OnNearbyItemEntered);
        EventBus.Subscribe<OnNearbyItemExit>(OnNearbyItemExit);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnNearbyItemEntered>(OnNearbyItemEntered);
        EventBus.Unsubscribe<OnNearbyItemExit>(OnNearbyItemExit);
        base.Disable();
    }

    public void Grab()
    {
        if(_nearbyItems.Count > 0)
        {
            Release();
            _grabbedItem = FindNearestItem();
            _grabbedItem.transform.parent = _handParent;
            _grabbedItem.transform.position = _handParent.position;
            _grabbedItem.BeGrabbed();
            RemoveNearbyItem(_grabbedItem);
        }
    }

    public void Release()
    {
        if (_grabbedItem)
        {
            _grabbedItem.transform.parent = _worldParent;
            _grabbedItem.BeReleased();
            _grabbedItem = null;
        }
    }

    private void OnNearbyItemEntered(in OnNearbyItemEntered onNearbyItemEntered)
    {
        AddNearbyItem(onNearbyItemEntered.item);
    }

    private void AddNearbyItem(RobotPart item)
    {
        if (!_nearbyItems.Contains(item))
            _nearbyItems.Add(item);
    }

    private void OnNearbyItemExit(in OnNearbyItemExit onNearbyItemExit)
    {
        RemoveNearbyItem(onNearbyItemExit.item);
    }

    private void RemoveNearbyItem(RobotPart item)
    {
        if (_nearbyItems.Contains(item))
            _nearbyItems.Remove(item);
    }

    private RobotPart FindNearestItem()
    {
        RobotPart nearestItem = _nearbyItems[0];
        foreach (RobotPart item in _nearbyItems)
        {
            if (nearestItem == item)
                continue;

            if (Vector3.Distance(_handParent.position, item.transform.position) < Vector3.Distance(_handParent.position, item.transform.position))
                nearestItem = item;
        }
        return nearestItem;
    }
}
