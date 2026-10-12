using UnityEngine;
using UnityEngine.UI;

public class PartButton : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private RobotPart.Type type;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        EventBus.Raise<OnPartRequest>(type);
    }
}