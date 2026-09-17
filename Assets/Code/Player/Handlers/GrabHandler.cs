using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Transform _worldParent;
    private Transform _handParent;

    private GameObject _grabbedItem;

    private List<GameObject> _nearbyItems;

    public GrabHandler(ref PlayerParents parents)
    {
        _worldParent = parents.worldParent;
        _handParent = parents.handParent;
    }

    public override void Enable()
    {
        base.Enable();
        EventBus.Subscribe<OnItemGrabbed>(Grab);
        EventBus.Subscribe<OnNearbyItemEntered>(OnNearbyItemEntered);
        EventBus.Subscribe<OnNearbyItemExit>(OnNearbyItemExit);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnItemGrabbed>(Grab);
        EventBus.Unsubscribe<OnNearbyItemEntered>(OnNearbyItemEntered);
        EventBus.Unsubscribe<OnNearbyItemExit>(OnNearbyItemExit);
        base.Disable();
    }

    public void Grab(in OnItemGrabbed onItemGrabbed)
    {
        _grabbedItem = FindNearestItem();
        _grabbedItem.transform.parent = _handParent;
        _grabbedItem.transform.position = _handParent.position;
        RemoveNearbyItem(_grabbedItem);
    }

    public void Release()
    {
        _grabbedItem.transform.parent = _worldParent;
        _grabbedItem = null;
    }

    private void OnNearbyItemEntered(in OnNearbyItemEntered onNearbyItemEntered)
    {
        AddNearbyItem(onNearbyItemEntered.item);
    }

    private void AddNearbyItem(GameObject item)
    {
        _nearbyItems.Add(item);
    }

    private void OnNearbyItemExit(in OnNearbyItemExit onNearbyItemExit)
    {
        RemoveNearbyItem(onNearbyItemExit.item);
    }

    private void RemoveNearbyItem(GameObject item)
    {
        _nearbyItems.Remove(item);
    }

    private GameObject FindNearestItem()
    {
        GameObject nearestItem = _nearbyItems[0];
        foreach (GameObject item in _nearbyItems)
        {
            if (nearestItem == item)
                continue;

            if (Vector3.Distance(_handParent.position, item.transform.position) < Vector3.Distance(_handParent.position, item.transform.position))
                nearestItem = item;
        }
        return nearestItem;
    }
}
