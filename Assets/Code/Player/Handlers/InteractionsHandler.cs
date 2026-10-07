using System.Collections.Generic;
using UnityEngine;

public class InteractionsHandler : PlayerHandler
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private PlayerParents _playerParents;

    private List<PushableObject> _nearbyPushableObjects = new List<PushableObject>();

    private PushableObject _grabbedPushableObejct;
    #endregion

    #region Initialization
    public InteractionsHandler(ref PlayerParents parents)
    {
        _playerParents = parents;

        _grabbedPushableObejct = null;
    }

    public override void Enable()
    {
        base.Enable();
        EventBus.Subscribe<OnPushableObjectEntered>(AddPushableObject);
        EventBus.Subscribe<OnPushableObjectExit>(RemovePushableObject);
        EventBus.Subscribe<OnPushRequest>(OnPushRequest);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnPushableObjectEntered>(AddPushableObject);
        EventBus.Unsubscribe<OnPushableObjectExit>(RemovePushableObject);
        EventBus.Unsubscribe<OnPushRequest>(OnPushRequest);
        base.Disable();
    } 
    #endregion

    private void AddPushableObject(in OnPushableObjectEntered context)
    {
        _nearbyPushableObjects.Add(context.pushableObject);
    }

    private void RemovePushableObject(in OnPushableObjectExit context)
    {
        _nearbyPushableObjects.Remove(context.pushableObject);
    }

    private void OnPushRequest(in OnPushRequest context)
    {
        if (_nearbyPushableObjects.Count > 0)
            EventBus.Raise<OnPushRequestInteractionsAccepted>();
    }

    public void GrabNearestPushableObject()
    {
        PushableObject nearestObject = FindNearestPushableObject();
        nearestObject.Grab();
        _grabbedPushableObejct = nearestObject;
    }

    public void ReleasePushableObject()
    {
        _grabbedPushableObejct?.Release();
        _grabbedPushableObejct = null;
    }

    public void PushObject(float delta, float speed)
    {
        _grabbedPushableObejct.Move(new Vector2(0, delta) * Time.deltaTime * speed);
    }

    public Vector3 GetOffsetedPositionFromPushableObejct()
    {
        if (_grabbedPushableObejct)
        {
            return _grabbedPushableObejct.transform.position - new Vector3(0, 0, 1);
        }
        return Vector3.zero;
    }

    private PushableObject FindNearestPushableObject()
    {
        PushableObject nearestItem = _nearbyPushableObjects[0];
        foreach (PushableObject item in _nearbyPushableObjects)
        {
            if (nearestItem == item)
                continue;

            if (Vector3.Distance(_playerParents.handParent.position, item.transform.position) < Vector3.Distance(_playerParents.handParent.position, item.transform.position))
                nearestItem = item;
        }
        return nearestItem;
    }
}