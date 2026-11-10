using UnityEngine;

public class RobotAudioPlayer : MonoBehaviour
{
    private EventBus EventBus =>
        ServiceProvider.Instance.GetService<EventBus>();
    private uint pushPlayingId;
    private uint chargePlayingId;
    public AK.Wwise.Event JumpEvent;
    public AK.Wwise.Event PushEvent;
    public AK.Wwise.Event SuperJumpChargeEvent;
    public AK.Wwise.Event GrabPartEvent;
    //public AK.Wwise.Event PartEquippedEvent;

    //Los coments son para des-comentar cuando este agregado el busevent OnPartEquipped.

    private void OnEnable()
    {
        EventBus.Subscribe<OnJumpStart>(PlayJumpAudio);
        EventBus.Subscribe<OnPushForward>(PlayPushAudio);
        EventBus.Subscribe<OnPushBack>(PlayPushAudio);
        EventBus.Subscribe<OnStopPushing>(StopPushAudio);
        EventBus.Subscribe<OnChargeStart>(PlaySuperJumpCharge);
        EventBus.Subscribe<OnPlayerStateChangeRequest>(StopPushOnStateChange);
        EventBus.Subscribe<OnPartGrabbed>(PlayGrabPartAudio);
        //EventBus.Subscribe<OnPartEquipped>(PlayEquipPartAudio);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnJumpStart>(PlayJumpAudio);
        EventBus.Unsubscribe<OnPushForward>(PlayPushAudio);
        EventBus.Unsubscribe<OnPushBack>(PlayPushAudio);
        EventBus.Unsubscribe<OnStopPushing>(StopPushAudio);
        EventBus.Unsubscribe<OnChargeStart>(PlaySuperJumpCharge);
        EventBus.Unsubscribe<OnPlayerStateChangeRequest>(StopPushOnStateChange);
        EventBus.Unsubscribe<OnPartGrabbed>(PlayGrabPartAudio);
        //EventBus.Unsubscribe<OnPartEquipped>(PlayEquipPartAudio);

        if (pushPlayingId != 0)
        {
            AkUnitySoundEngine.StopPlayingID(pushPlayingId);
            pushPlayingId = 0;
        }
        if (chargePlayingId != 0)
        {
            AkUnitySoundEngine.StopPlayingID(chargePlayingId);
            chargePlayingId = 0;
        }

    }
    /*private void PlayEquipPartAudio(in OnPartEquipped context)
    {
        PartEquippedEvent.Post(gameObject);
    }*/
    private void PlayGrabPartAudio(in OnPartGrabbed context)
    {
        GrabPartEvent.Post(gameObject);
    }
    private void StopPushOnStateChange(in OnPlayerStateChangeRequest context)
    {
        if (context.state == BehaviourFSM.State.Idle && pushPlayingId != 0)
        {
            AkUnitySoundEngine.StopPlayingID(pushPlayingId);
            pushPlayingId = 0;
        }
    }
    private void PlaySuperJumpCharge(in OnChargeStart context)
    {
        if (chargePlayingId == 0)
            chargePlayingId = SuperJumpChargeEvent.Post(gameObject);

    }

    private void PlayJumpAudio(in OnJumpStart context)
    {
        JumpEvent.Post(gameObject);
        if (chargePlayingId != 0)
        {
            AkUnitySoundEngine.StopPlayingID(chargePlayingId);
            chargePlayingId = 0;
        }
        
    }

    private void PlayPushAudio<T>(in T context)
    {
        if (pushPlayingId == 0)
            pushPlayingId = PushEvent.Post(gameObject);
    }
    private void StopPushAudio(in OnStopPushing context)
    {
        if (pushPlayingId != 0)
        {
            AkUnitySoundEngine.StopPlayingID(pushPlayingId);
            pushPlayingId = 0;
        }
    }
}
