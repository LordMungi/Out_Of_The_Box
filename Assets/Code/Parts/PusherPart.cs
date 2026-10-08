using UnityEngine;
using UnityEngine.InputSystem;

public class PusherPart : RobotPart
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private bool _partsHasAccepted = false;
    private bool _interactionsHasAccepted = false;

    private void OnEnable()
    {
        EventBus.Subscribe<OnPushRequestInteractionsAccepted>(OnInteractionsAccepted);
        EventBus.Subscribe<OnPushRequestPartsAccepted>(OnPartsAccepted);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPushRequestInteractionsAccepted>(OnInteractionsAccepted);
        EventBus.Unsubscribe<OnPushRequestPartsAccepted>(OnPartsAccepted);
    }

    private void Update()
    {
        _interactionsHasAccepted = false;
        _partsHasAccepted = false;
    }

    protected override void SetActionPair()
    {
        type = Type.Pusher;

        actions.Add(new ActionPair(_inputActions.Player.Grab, ActionPair.Phase.Performed, RequestPush));
    }

    private void RequestPush(InputAction.CallbackContext callbackContext)
    {
        EventBus.Raise<OnPushRequest>();
    }

    private void OnPartsAccepted(in OnPushRequestPartsAccepted context)
    {
        _partsHasAccepted = true;

        if (AllRequestsAccepted())
            RaisePushAccepted();
    }

    private void OnInteractionsAccepted (in OnPushRequestInteractionsAccepted context)
    {
        _interactionsHasAccepted = true;

        if (AllRequestsAccepted())
            RaisePushAccepted();
    }

    private bool AllRequestsAccepted()
    {
        return _partsHasAccepted && _interactionsHasAccepted;
    }

    private void RaisePushAccepted()
    {
        Debug.Log("Push Accepted");
        EventBus.Raise<OnPartStateChangeAccepted>(BehaviourFSM.State.Push);
    }
}
