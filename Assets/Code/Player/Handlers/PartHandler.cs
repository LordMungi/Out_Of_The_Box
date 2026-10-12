using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Transform _worldParent;
    private Transform _underWorldParent;
    private Transform _handParent;
    private List<EquippedPart> _equippedParts;

    private RobotPart _grabbedPart;
    public RobotPart _equippedPartArms;
    public RobotPart _equippedPartLegs;

    public RobotPart EquippedPartArms
    {
        get => _equippedPartArms;
    }
    public RobotPart EquippedPartLegs
    {
        get => _equippedPartLegs;
    }

    private static List<RobotPart> _nearbyParts = new List<RobotPart>();

    private PlayerInputActions _inputActions;

    public PartHandler(ref PlayerParents parents, EquippedPart[] equippedParts, PlayerInputActions inputActions)
    {
        _worldParent = parents.worldParent;
        _underWorldParent = parents.underWorldParent;
        _handParent = parents.handParent;
        _equippedParts = new List<EquippedPart>(equippedParts);

        _inputActions = inputActions;
    }

    public override void Enable()
    {
        base.Enable();
        EventBus.Subscribe<OnNearbyPartEntered>(OnNearbyPartEntered);
        EventBus.Subscribe<OnNearbyPartExit>(OnNearbyPartExit);
        EventBus.Subscribe<OnPushRequest>(OnPushRequest);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnNearbyPartEntered>(OnNearbyPartEntered);
        EventBus.Unsubscribe<OnNearbyPartExit>(OnNearbyPartExit);
        EventBus.Unsubscribe<OnPushRequest>(OnPushRequest);
        base.Disable();
    }

    private void Grab(RobotPart part)
    {
        _grabbedPart = part;
        _grabbedPart.transform.parent = _handParent;
        _grabbedPart.transform.position = _handParent.position;
        _grabbedPart.transform.rotation = _handParent.rotation;
        _grabbedPart.Grab();
        EventBus.Raise<OnPartGrabbed>(_grabbedPart);
        RemoveNearbyItem(_grabbedPart);
    }

    public void GrabAndRelease(RobotPart part)
    {
        Release();
        if (part != _equippedPartArms && part != _equippedPartLegs)
            Grab(part);
    }

    public bool GrabNearest()
    {
        if (_nearbyParts.Count > 0)
        {
            Release();
            Grab(FindNearestPart());
            return true;
        }
        return false;
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
        if (_grabbedPart)
        {
            RobotPart auxGrabbedPart = _grabbedPart;
            bool isLegPart = _grabbedPart?.type == RobotPart.Type.Jumper ||
                             _grabbedPart?.type == RobotPart.Type.Climber ||
                             _grabbedPart?.type == RobotPart.Type.Glider;

            RobotPart targetPart = isLegPart ? _equippedPartLegs : _equippedPartArms;

            if (targetPart)
            {
                Grab(targetPart);
                UnsubscribeFromPartActions(targetPart);
                targetPart = null;
            }

            if (auxGrabbedPart)
            {
                if (isLegPart)
                    EquipOnLegs(auxGrabbedPart);
                else
                    EquipOnArms(auxGrabbedPart);

                if (auxGrabbedPart == _grabbedPart)
                    _grabbedPart = null;
            }
        }
        else
        {
            if (_equippedPartArms)
            {
                Grab(_equippedPartArms);
                UnsubscribeFromPartActions(_equippedPartArms);
                _equippedPartArms = null;
            }
            else if (_equippedPartLegs)
            {
                Grab(_equippedPartLegs);
                UnsubscribeFromPartActions(_equippedPartLegs);
                _equippedPartLegs = null;
            }
        }

        SwapPartModels();
    }

    private void EquipOnArms(RobotPart part)
    {
        _equippedPartArms = part;
        _equippedPartArms.Equip();
        _equippedPartArms.transform.parent = _underWorldParent;
        _equippedPartArms.transform.position = _underWorldParent.position;
        SubscribeToPartActions(_equippedPartArms);
    }

    private void EquipOnLegs(RobotPart part)
    {
        _equippedPartLegs = part;
        _equippedPartLegs.Equip();
        _equippedPartLegs.transform.parent = _underWorldParent;
        _equippedPartLegs.transform.position = _underWorldParent.position;
        SubscribeToPartActions(_equippedPartLegs);
    }

    private void SwapPartModels()
    {
        bool legPartEquipped = _equippedPartLegs != null;
        bool armPartEquipped = _equippedPartArms != null;

        foreach (EquippedPart part in _equippedParts)
        {
            part.gameObject.SetActive(false);

            if (!legPartEquipped)
            {
                if (part.type == RobotPart.Type.LegBase)
                    part.gameObject.SetActive(true);
            }
            if (!armPartEquipped)
            {
                if (part.type == RobotPart.Type.ArmBase)
                    part.gameObject.SetActive(true);
            }
            if (part.type == _equippedPartArms?.type || part.type == _equippedPartLegs?.type)
                part.gameObject.SetActive(true);
        }
    }

    private void OnNearbyPartEntered(in OnNearbyPartEntered onNearbyItemEntered)
    {
        AddNearbyItem(onNearbyItemEntered.item);
    }

    private void AddNearbyItem(RobotPart part)
    {
        if (!_nearbyParts.Contains(part) && part != _grabbedPart && part != _equippedPartArms)
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

    private void OnPushRequest(in OnPushRequest context)
    {
        if (_grabbedPart == null)
            EventBus.Raise<OnPushRequestPartsAccepted>();
    }
}
