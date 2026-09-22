using UnityEngine;
using UnityEngine.UI;

public class VersionText : MonoBehaviour
{
    [SerializeField] private Text text;

    void Start()
    {
        text.text = Application.version;
    }
}
