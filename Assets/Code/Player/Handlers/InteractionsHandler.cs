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

    private ClimbableObject _grabbedClimbableObject;
    public ClimbableObject GrabbedClimbableObject { get { return _grabbedClimbableObject; } private set { } }

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
        offsetFromPushableObject = new Vector3(offsetFromPushableObject.x, 0, offsetFromPushableObject.z);

        Vector3 newDirection;

        float dotFront = Vector3.Dot(offsetFromPushableObject, nearestObject.transform.forward);
        float dotBack = Vector3.Dot(offsetFromPushableObject, -nearestObject.transform.forward);
        float dotRight = Vector3.Dot(offsetFromPushableObject, nearestObject.transform.right);
        float dotLeft = Vector3.Dot(offsetFromPushableObject, -nearestObject.transform.right);

        float maxDot = Mathf.Max(dotFront, dotBack, dotRight, dotLeft);
        
        if (maxDot == dotFront)
            newDirection = nearestObject.transform.forward; // Front 
        else if (maxDot == dotBack)
            newDirection = -nearestObject.transform.forward; // Back
        else if (maxDot == dotRight)
            newDirection = nearestObject.transform.right; // Right
        else
            newDirection = -nearestObject.transform.right; // Left

        _directionVectorPushableObject = new Vector2(newDirection.x, newDirection.z);
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

    #region Climbable Objects
    public void ClimbNearestObject()
    {
        _grabbedClimbableObject = FindNearestObject(_nearbyClimbableObjects);
    }

    public void StopClimbing()
    {
        _grabbedClimbableObject = null;
    }

    public bool ShouldGetOffOnTop()
    {
        return _grabbedClimbableObject.IsPlayerOnTop;
    }

    public Vector3 GetOffsetedPosition(Vector3 position)
    {
        return _grabbedClimbableObject.WallPlane.ClosestPointOnPlane(position) + _grabbedClimbableObject.WallPlane.normal * _grabbedClimbableObject.OffsetFromWall;
    }

    public Vector3 GetOffsetedTopPosition(Vector3 position)
    {
        Vector3 newVector = _grabbedClimbableObject.WallPlane.ClosestPointOnPlane(position) + _grabbedClimbableObject.WallPlane.normal * _grabbedClimbableObject.OffsetFromTop;
        return new Vector3(newVector.x, _grabbedClimbableObject.TopOffset.position.y, newVector.z);
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