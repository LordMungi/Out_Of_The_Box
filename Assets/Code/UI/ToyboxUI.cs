using System.Collections.Generic;
using UnityEngine;

public class ToyboxUI : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private Canvas canvasParent;
    [SerializeField] private PartButton[] buttons;

    private void OnEnable()
    {
        EventBus.Subscribe<OnToyboxStateEnabled>(Activate);
        EventBus.Subscribe<OnToyboxStateDisabled>(Deactivate);
        EventBus.Subscribe<OnPartGrabbed>(AddPart);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnToyboxStateEnabled>(Activate);
        EventBus.Unsubscribe<OnToyboxStateDisabled>(Deactivate);
        EventBus.Unsubscribe<OnPartGrabbed>(AddPart);
    }

    private void Activate(in OnToyboxStateEnabled context)
    {
        canvasParent.enabled = true;
    }

    private void Deactivate(in OnToyboxStateDisabled context)
    {
        canvasParent.enabled = false;
    }

    private void AddPart(in OnPartGrabbed context)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].type == context.part.type)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }
    }
}