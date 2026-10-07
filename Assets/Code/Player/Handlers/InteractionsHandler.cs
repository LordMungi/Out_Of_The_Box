using System.Collections.Generic;
using UnityEngine;

public class InteractionsHandler : PlayerHandler
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private PlayerParents _playerParents;

    private List<PushableObject> _nearbyPushableObjects = new List<PushableObject>();

    private PushableObject _grabbedPushableObejct;
    private Vector3 _offsetFromPushableObject;
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

    public void GrabNearestPushableObject(Vector3 playerPosition)
    {
        PushableObject nearestObject = FindNearestPushableObject();
        nearestObject.Grab();
        _grabbedPushableObejct = nearestObject;
        _offsetFromPushableObject = _grabbedPushableObejct.transform.position - playerPosition;

        Vector2 absOffset = new Vector2(Mathf.Abs(_offsetFromPushableObject.x), Mathf.Abs(_offsetFromPushableObject.z));

        if (absOffset.x > absOffset.y)
        {
            if (_offsetFromPushableObject.x > 0)
                _directionFromPushableObject = Direction.Left;
            else
                _directionFromPushableObject = Direction.Right;
        }
        else
        {
            if (_offsetFromPushableObject.z > 0)
                _directionFromPushableObject = Direction.Front;
            else
                _directionFromPushableObject = Direction.Back;
        }
    }

    public void ReleasePushableObject()
    {
        _grabbedPushableObejct?.Release();
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
    public Vector3 GetOffsetedPositionFromPushableObejct()
    {
        if (_grabbedPushableObejct)
        {
            return _grabbedPushableObejct.transform.position - _offsetFromPushableObject;
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