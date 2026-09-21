using System.Collections.Generic;
using UnityEngine;

public class PartHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Transform _worldParent;
    private Transform _handParent;
    private Transform _legPartParent;
    private Transform _armPartParent;

    private static RobotPart _grabbedPart;
    private static RobotPart _equippedPart;

    private static List<RobotPart> _nearbyParts = new List<RobotPart>();

    public PartHandler(ref PlayerParents parents)
    {
        _worldParent = parents.worldParent;
        _handParent = parents.handParent;
        _legPartParent = parents.legPartParent;
        _armPartParent = parents.armPartParent;
    }

    public override void Enable()
    {
        base.Enable();
        EventBus.Subscribe<OnNearbyPartEntered>(OnNearbyPartEntered);
        EventBus.Subscribe<OnNearbyPartExit>(OnNearbyPartExit);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnNearbyPartEntered>(OnNearbyPartEntered);
        EventBus.Unsubscribe<OnNearbyPartExit>(OnNearbyPartExit);
        base.Disable();
    }

    private void Grab(RobotPart part)
    {
        _grabbedPart = part;
        _grabbedPart.transform.parent = _handParent;
        _grabbedPart.transform.position = _handParent.position;
        _grabbedPart.Grab();
        RemoveNearbyItem(_grabbedPart);
    }

    public void GrabNearest()
    {
        if(_nearbyParts.Count > 0)
        {
            Release();
            Grab(FindNearestPart());
        }
    }

    public void Release()
    {
        if (_grabbedPart)
        {
            _grabbedPart.transform.parent = _worldParent;
            _grabbedPart.Release();
            _grabbedPart = null;
        }
    }

    public void EquipGrabbed()
    {
        RobotPart auxPart = _equippedPart;

        if (_grabbedPart)
        {
            _equippedPart = _grabbedPart;
            _grabbedPart = null;

            _equippedPart.Equip();
            _equippedPart.transform.parent = _legPartParent;
            _equippedPart.transform.position = _legPartParent.position;
        }

        if (auxPart)
        {
            Grab(auxPart);
            if (auxPart == _equippedPart)
               _equippedPart = null;
        }


    }

    private void OnNearbyPartEntered(in OnNearbyPartEntered onNearbyItemEntered)
    {
        AddNearbyItem(onNearbyItemEntered.item);
    }

    private void AddNearbyItem(RobotPart part)
    {
        if (!_nearbyParts.Contains(part) && part != _grabbedPart && part != _equippedPart)
            _nearbyParts.Add(part);
    }

    private void OnNearbyPartExit(in OnNearbyPartExit onNearbyItemExit)
    {
        RemoveNearbyItem(onNearbyItemExit.item);
    }

    private void RemoveNearbyItem(RobotPart part)
    {
        if (_nearbyParts.Contains(part))
            _nearbyParts.Remove(part);
    }

    private RobotPart FindNearestPart()
    {
        RobotPart nearestItem = _nearbyParts[0];
        foreach (RobotPart item in _nearbyParts)
        {
            if (nearestItem == item)
                continue;

            if (Vector3.Distance(_handParent.position, item.transform.position) < Vector3.Distance(_handParent.position, item.transform.position))
                nearestItem = item;
        }
        return nearestItem;
    }
}
