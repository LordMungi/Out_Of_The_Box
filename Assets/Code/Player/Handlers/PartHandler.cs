using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Transform _worldParent;
    private Transform _handParent;
    private Transform _legPartParent;
    private Transform _armPartParent;

    private static RobotPart _grabbedPart;
    public static RobotPart _equippedPart;

    public RobotPart EquippedPart
    {
        get => _equippedPart;
    }

    private static List<RobotPart> _nearbyParts = new List<RobotPart>();

    private PlayerInputActions _inputActions;

    public PartHandler(ref PlayerParents parents, PlayerInputActions inputActions)
    {
        _worldParent = parents.worldParent;
        _handParent = parents.handParent;
        _legPartParent = parents.legPartParent;
        _armPartParent = parents.armPartParent;

        _inputActions = inputActions;
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
        RobotPart auxGrabbedPart = _grabbedPart;

        if (_equippedPart)
        {
            Grab(_equippedPart);
            UnsubscribeFromPartActions(_equippedPart);
            _equippedPart = null;
        }

        if (auxGrabbedPart)
        {
            Equip(auxGrabbedPart);
            if (auxGrabbedPart == _grabbedPart)
                _grabbedPart = null;
        }
    }

    private void Equip(RobotPart part)
    {
        _equippedPart = part;
        _equippedPart.Equip();
        _equippedPart.transform.parent = _legPartParent;
        _equippedPart.transform.position = _legPartParent.position;
        SubscribeToPartActions(_equippedPart);
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

    private void SubscribeToPartActions(RobotPart part)
    {
        for (int i = 0; i < part.actions.Count; i++)
        {
            RobotPart.ActionPair newActionPair = part.actions[i];
            switch (newActionPair.phase)
            {
                case RobotPart.ActionPair.Phase.Started:
                    newActionPair.inputAction.started += newActionPair.triggeredAction;
                    break;
                case RobotPart.ActionPair.Phase.Performed:
                    newActionPair.inputAction.performed += newActionPair.triggeredAction;
                    break;
                case RobotPart.ActionPair.Phase.Canceled:
                    newActionPair.inputAction.canceled += newActionPair.triggeredAction;
                    break;
                default:
                    break;
            }
            part.actions[i] = newActionPair;
        }
    }

    private void UnsubscribeFromPartActions(RobotPart part)
    {
        for (int i = 0; i < part.actions.Count; i++)
        {
            RobotPart.ActionPair newActionPair = part.actions[i];
            switch (newActionPair.phase)
            {
                case RobotPart.ActionPair.Phase.Started:
                    newActionPair.inputAction.started -= newActionPair.triggeredAction;
                    break;
                case RobotPart.ActionPair.Phase.Performed:
                    newActionPair.inputAction.performed -= newActionPair.triggeredAction;
                    break;
                case RobotPart.ActionPair.Phase.Canceled:
                    newActionPair.inputAction.canceled -= newActionPair.triggeredAction;
                    break;
                default:
                    break;
            }
            part.actions[i] = newActionPair;
        }
    }
}
