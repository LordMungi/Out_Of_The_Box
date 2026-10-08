using System.Collections.Generic;
using UnityEngine;

public class InteractionsHandler : PlayerHandler
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Transform _handParent;
    private Transform _worldParent;

    private List<PushableObject> _nearbyPushableObjects = new List<PushableObject>();

    private PushableObject _grabbedPushableObejct;
    private Direction _directionFromPushableObject;

    private enum Direction
    {
        Front,
        Back,
        Left,
        Right
    }
    #endregion

    #region Initialization
    public InteractionsHandler(ref PlayerParents parents)
    {
        _handParent = parents.handParent;
        _worldParent = parents.worldParent;

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

    public void GrabNearestPushableObject(Vector3 playerPosition)
    {
        PushableObject nearestObject = FindNearestPushableObject();
        Grab(nearestObject);

        Vector3 offsetFromPushableObject = _grabbedPushableObejct.transform.position - playerPosition;

        Vector2 absOffset = new Vector2(Mathf.Abs(offsetFromPushableObject.x), Mathf.Abs(offsetFromPushableObject.z));

        if (absOffset.x > absOffset.y)
        {
            if (offsetFromPushableObject.x > 0)
                _directionFromPushableObject = Direction.Left;
            else
                _directionFromPushableObject = Direction.Right;
        }
        else
        {
            if (offsetFromPushableObject.z > 0)
                _directionFromPushableObject = Direction.Front;
            else
                _directionFromPushableObject = Direction.Back;
        }
    }

    private void Grab(PushableObject pushableObject)
    {
        _grabbedPushableObejct = pushableObject;
        _grabbedPushableObejct.Grab();
        _grabbedPushableObejct.transform.parent = _handParent;
    }

    public void ReleasePushableObject()
    {
        _grabbedPushableObejct?.Release();
        _grabbedPushableObejct.transform.parent = _worldParent;
        _grabbedPushableObejct = null;
    }

    public void PushObject(float delta, float speed)
    {
        Vector2 moveVector;
        switch (_directionFromPushableObject)
        {
            case Direction.Front:
                moveVector = new Vector2(0, delta);
                break;
            case Direction.Back:
                moveVector = new Vector2(0, -delta);
                break;
            case Direction.Left:
                moveVector = new Vector2(delta, 0);
                break;
           case Direction.Right:
                moveVector = new Vector2(-delta, 0);
                break; 
            default:                moveVector = new Vector2();
                break; 
        }
        _grabbedPushableObejct.Move(moveVector * Time.deltaTime * speed);
    }

    public bool CanPushObject(Vector2 delta)
    {
        return _grabbedPushableObejct.CanMove(delta);
    }

    private PushableObject FindNearestPushableObject()
    {
        PushableObject nearestItem = _nearbyPushableObjects[0];
        foreach (PushableObject item in _nearbyPushableObjects)
        {
            if (nearestItem == item)
                continue;

            if (Vector3.Distance(_handParent.position, item.transform.position) < Vector3.Distance(_handParent.position, item.transform.position))
                nearestItem = item;
        }
        return nearestItem;
    }
}