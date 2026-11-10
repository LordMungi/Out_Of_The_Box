using UnityEngine;
using TMPro;

public class VersionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        text.text = Application.version;
    }
}
