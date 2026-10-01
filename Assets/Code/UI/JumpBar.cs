using UnityEngine;
using UnityEngine.UI;

public class JumpBar : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private PlayerConfig config;
    [SerializeField] private Image bar;

    void Start()
    {
        EventBus.Subscribe<OnSuperJumpChargeUpdated>(OnSuperJumpChargeUpdated);
        EventBus.Subscribe<OnSuperJumpChargeEnded>(OnSuperJumpChargeEnded);
        bar.enabled = false;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnSuperJumpChargeUpdated>(OnSuperJumpChargeUpdated);
        EventBus.Unsubscribe<OnSuperJumpChargeEnded>(OnSuperJumpChargeEnded);
    }

    private void OnSuperJumpChargeUpdated(in OnSuperJumpChargeUpdated context)
    {
        if (!bar.enabled)
            bar.enabled = true;

        bar.fillAmount = context.newValue / config.superJumpMaxTime;
    }

    private void OnSuperJumpChargeEnded(in OnSuperJumpChargeEnded context)
    {
        bar.enabled = false;
    }
}
