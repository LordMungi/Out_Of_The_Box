#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace DebugToolkit.Editor
{
    [InitializeOnLoad]
    public class WelcomePopup : EditorWindow
    {
        private const string DontShowAgainKey = "UDT_Welcome";
        private bool _dontShowAgain;
        private static AssetState _assetState;

        [MenuItem("Tools/4H2C/Free Cam/DebugToolkitInfo")]
        private static void Open()
        {
            OpenWindow();
            _assetState = Resources.Load<AssetState>("AssetStateSO");
        }

        [InitializeOnLoadMethod]
        private static void ShowOnFirstOpen()
        {
            if (!EditorPrefs.HasKey(DontShowAgainKey))
            {
                OpenWindow();
            }
        }

        public static void OpenWindow()
        {
            WelcomePopup window = GetWindow<WelcomePopup>("Info");
            window.position = new Rect(Screen.width / 2, Screen.height / 1.25f, 400, 650);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Welcome to UDT", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Thank you for using UDT. Here's how to get started:");
            EditorGUILayout.Space();

            if (GUILayout.Button("Open Documentation"))
            {
                Application.OpenURL("https://4hands2cats.github.io/UnityDebugToolkitDocumentation/#/");
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Join the discord server"))
            {
                Application.OpenURL("https://discord.gg/6gWfe4epyr");
            }

            EditorGUILayout.Space();

            ShowAd();

            EditorGUILayout.Space();
            GUILayout.FlexibleSpace();


            _dontShowAgain = EditorGUILayout.ToggleLeft("Don't show this again", _dontShowAgain);

            EditorGUILayout.Space();

            if (GUILayout.Button("Close"))
            {
                if (_dontShowAgain)
                {
                    EditorPrefs.SetBool(DontShowAgainKey, true);
                }
                Close();
            }
        }

        private void ShowAd()
        {
            if (_assetState.assetState == AssetState.EAssetState.DebugToolkit) return;

            EditorGUILayout.LabelField("Come check out our other Assets", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUI.DrawPreviewTexture(
                GUILayoutUtility.GetRect(200, 200),
                _assetState.debugToolkit.texture,
                null,
                ScaleMode.ScaleToFit
            );
            int discount = _assetState.assetState == AssetState.EAssetState.Camera ? 10 : 15;
            if (GUILayout.Button($"Come Check the Debug Toolkit with {discount} $ discount"))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/debug-toolkit-in-game-console-309702");
            }
            EditorGUILayout.Space();

            if (_assetState.assetState == AssetState.EAssetState.ReportToolkit) return;

            EditorGUI.DrawPreviewTexture(
                GUILayoutUtility.GetRect(200, 200),
                _assetState.reportToolkit.texture,
                null,
                ScaleMode.ScaleToFit
            );
            if (GUILayout.Button("Come Check the Report Toolkit with 5 $ discount"))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/ultimate-bug-feedback-report-318522");
            }
        }
    }
}

#endif