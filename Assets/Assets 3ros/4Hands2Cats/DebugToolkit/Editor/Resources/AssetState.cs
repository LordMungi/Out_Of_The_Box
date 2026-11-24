using UnityEngine;

[CreateAssetMenu(fileName = "AssetState", menuName = "DebugToolkit/Editor/AssetState")]
public class AssetState : ScriptableObject
{
    public enum EAssetState
    {
        DebugToolkit,
        Camera,
        ReportToolkit
    }

    public EAssetState assetState;
    public Sprite debugToolkit;
    public Sprite reportToolkit;
}
