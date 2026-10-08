using System.Collections.Generic;
using UnityEngine;

public class InteractionsHandler : PlayerHandler
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Transform _handParent;
    private Transform _worldParent;

    private List<PushableObject> _nearbyPushableObjects = new List<PushableObject>();
    private List<ClimbableObject> _nearbyClimbableObjects = new List<ClimbableObject>();

    private PushableObject _grabbedPushableObejct;
    private Vector2 _directionVectorPushableObject;

    public Vector2 DirectionVectorPushableObject { get { return _directionVectorPushableObject; } private set { } }

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
        EventBus.Subscribe<OnClimbableObjectEntered>(AddClimbableObject);
        EventBus.Subscribe<OnClimbableObjectExit>(RemoveClimbableObject);
        EventBus.Subscribe<OnPushRequest>(OnPushRequest);
        EventBus.Subscribe<OnClimbRequest>(OnClimbRequest);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnPushableObjectEntered>(AddPushableObject);
        EventBus.Unsubscribe<OnPushableObjectExit>(RemovePushableObject);
        EventBus.Unsubscribe<OnClimbableObjectEntered>(AddClimbableObject);
        EventBus.Unsubscribe<OnClimbableObjectExit>(RemoveClimbableObject);
        EventBus.Unsubscribe<OnPushRequest>(OnPushRequest);
        EventBus.Unsubscribe<OnClimbRequest>(OnClimbRequest);
        base.Disable();
    }
    #endregion

    #region Callbacks
    private void AddPushableObject(in OnPushableObjectEntered context)
    {
        _nearbyPushableObjects.Add(context.pushableObject);
    }

    private void RemovePushableObject(in OnPushableObjectExit context)
    {
        _nearbyPushableObjects.Remove(context.pushableObject);
    }

    private void AddClimbableObject(in OnClimbableObjectEntered context)
    {
        _nearbyClimbableObjects.Add(context.climbableObject);
    }

    private void RemoveClimbableObject(in OnClimbableObjectExit context)
    {
        _nearbyClimbableObjects.Remove(context.climbableObject);
    }

    private void OnPushRequest(in OnPushRequest context)
    {
        if (_nearbyPushableObjects.Count > 0)
            EventBus.Raise<OnPushRequestInteractionsAccepted>();
    }

    private void OnClimbRequest(in OnClimbRequest context)
    {
        Debug.Log("Climb Request: " + _nearbyClimbableObjects.Count);
        if (_nearbyClimbableObjects.Count > 0)
            EventBus.Raise<OnPartStateChangeAccepted>(BehaviourFSM.State.Climb);
    }
    #endregion

    #region Pushable Objects
    public void GrabNearestPushableObject(Vector3 playerPosition)
    {
        PushableObject nearestObject = FindNearestObject(_nearbyPushableObjects);
        Grab(nearestObject);

        Vector3 offsetFromPushableObject = _grabbedPushableObejct.transform.position - playerPosition;

        Vector2 absOffset = new Vector2(Mathf.Abs(offsetFromPushableObject.x), Mathf.Abs(offsetFromPushableObject.z));

        if (absOffset.x > absOffset.y)
        {
            if (offsetFromPushableObject.x > 0)
                _directionVectorPushableObject = new Vector2(1, 0); // Left
            else
                _directionVectorPushableObject = new Vector2(-1, 0); // Right
        }
        else
        {
            if (offsetFromPushableObject.z > 0)
                _directionVectorPushableObject = new Vector2(0, 1); // Front 
            else
                _directionVectorPushableObject = new Vector2(0, -1); // Back
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

    public bool CanPushObject(Vector2 delta)
    {
        return _grabbedPushableObejct.CanMove(delta);
    } 
    #endregion

    private T FindNearestObject<T>(List<T> list) where T : MonoBehaviour
    {
        T nearestItem = list[0];
        foreach (T item in list)
        {
            if (nearestItem == item)
                continue;

            if (Vector3.Distance(_handParent.position, item.transform.position) < Vector3.Distance(_handParent.position, item.transform.position))
                nearestItem = item;
        }
        return nearestItem;
    }
}