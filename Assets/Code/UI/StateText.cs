using UnityEngine;
using UnityEngine.UI;

public class StateText : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private Text _text;

    private void Start()
    {
        EventBus.Subscribe<OnPlayerStateChangeRequest>(ChangeStateText);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPlayerStateChangeRequest>(ChangeStateText);
    }

    private void ChangeStateText(in OnPlayerStateChangeRequest newStateEvent)
    {
        _text.text = "State: " + newStateEvent.state.ToString();
    }
}
