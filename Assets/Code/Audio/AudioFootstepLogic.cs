using System.Collections;
using UnityEngine;

public class RobotFootstepsController : MonoBehaviour
{
    private EventBus EventBus =>
        ServiceProvider.Instance.GetService<EventBus>();
    public AK.Wwise.Event FootstepEvent;

    [SerializeField] private float footstepInterval = 0.4f;

    private Coroutine footstepCoroutine;

    private void OnEnable()
    {
        EventBus.Subscribe<OnStartMoving>(StartFootsteps);
        EventBus.Subscribe<OnStartIdle>(StopFootsteps);
        EventBus.Subscribe<OnJumpStart>(StopFootsteps);
        EventBus.Subscribe<OnSuperJumpRequest>(StopFootsteps);

    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnStartMoving>(StartFootsteps);
        EventBus.Unsubscribe<OnStartIdle>(StopFootsteps);
        EventBus.Unsubscribe<OnJumpStart>(StopFootsteps);
        EventBus.Subscribe<OnSuperJumpRequest>(StopFootsteps);

        StopFootsteps();
    }

    private void StartFootsteps(in OnStartMoving context)
    {
        if (footstepCoroutine == null)
            footstepCoroutine = StartCoroutine(FootstepLoop());
    }
    private void StopFootsteps(in OnSuperJumpRequest context)
    {
        StopFootsteps();
    }
    private void StopFootsteps(in OnStartIdle context)
    {
        StopFootsteps();
    }
    private void StopFootsteps(in OnJumpStart context)
    {
        StopFootsteps();
    }

    private void StopFootsteps()
    {
        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
    }
    private IEnumerator FootstepLoop()
    {
        while (true)
        {
            FootstepEvent.Post(gameObject);

            yield return new WaitForSeconds(footstepInterval);
        }
    }
}