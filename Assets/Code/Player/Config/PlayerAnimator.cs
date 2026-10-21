using UnityEngine;

public class PlayerAnimator
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Animator _animator;

    private enum LegState
    {
        Idle,
        Move,
        ChargeJump,
        Climb,
        JumpDown,
        JumpUp,
        PushForward,
        PushBack
    }

    private enum ArmState
    {
        Idle,
        Shoot
    }

    public PlayerAnimator(Animator animator)
    {
        _animator = animator;

        EventBus.Subscribe<OnStartIdle>(Idle);
        EventBus.Subscribe<OnStartMoving>(Move);
        EventBus.Subscribe<OnStartClimb>(Climb);
        EventBus.Subscribe<OnStopClimb>(StopClimb);
        EventBus.Subscribe<OnPushForward>(PushForward);
        EventBus.Subscribe<OnPushBack>(PushBack);
        EventBus.Subscribe<OnStopPushing>(StopPush);
        EventBus.Subscribe<OnShoot>(Shoot);
    }

    ~PlayerAnimator()
    {
        EventBus.Unsubscribe<OnStartIdle>(Idle);
        EventBus.Unsubscribe<OnStartMoving>(Move);
        EventBus.Unsubscribe<OnStartClimb>(Climb);
        EventBus.Unsubscribe<OnPushForward>(PushForward);
        EventBus.Unsubscribe<OnPushBack>(PushBack);
        EventBus.Unsubscribe<OnStopPushing>(StopPush);
        EventBus.Unsubscribe<OnShoot>(Shoot);
    }

    #region Methods
    private void ChangeLegState(LegState state)
    {
        _animator.SetInteger("LegsState", (int)state);
    }

    private void ChangeArmState(ArmState state)
    {
        _animator.SetInteger("ArmsState", (int)state);
    } 
    #endregion

    #region Callbacks
    private void Idle(in OnStartIdle context)
    {
        ChangeLegState(LegState.Idle);
        ChangeArmState(ArmState.Idle);
    }
    private void Move(in OnStartMoving context)
    {
        ChangeLegState(LegState.Move);
    }
    private void Climb(in OnStartClimb context)
    {
        ChangeLegState(LegState.Climb);
    }
    private void StopClimb(in OnStopClimb context)
    {
        ChangeLegState(LegState.Idle);
        ChangeArmState(ArmState.Idle);
    }
    private void PushForward(in OnPushForward context)
    {
        ChangeLegState(LegState.PushForward);
    }
    private void PushBack(in OnPushBack context)
    {
        ChangeLegState(LegState.PushBack);
    }
    private void StopPush(in OnStopPushing context)
    {
        ChangeLegState(LegState.Idle);
    }
    private void Shoot(in OnShoot context)
    {
        _animator.SetTrigger("ShootTrigger");
    }
    #endregion
}
