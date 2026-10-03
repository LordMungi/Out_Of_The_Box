using UnityEngine;

public class PlayerAnimator
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Animator _animator;
    public PlayerAnimator(Animator animator)
    {
        _animator = animator;

        EventBus.Subscribe<OnPlayerStateChange>(OnStateChange);
    }

    ~PlayerAnimator()
    {
        EventBus.Unsubscribe<OnPlayerStateChange>(OnStateChange);
    }

    private void OnStateChange(in OnPlayerStateChange context)
    {
        _animator.SetInteger("CurrentState", (int)context.state);
    }
}
