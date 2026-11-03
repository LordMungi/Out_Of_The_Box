using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public AK.Wwise.Event sfxStartGame; 

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private float minLoadTime = 1.0f;
    [SerializeField] private MenuManager manager;

    private bool _isLoading = false;
    
    public void StartGame(string gameplaySceneName)
    {
        manager.ClearStack();
        
        if (_isLoading)
            return;

        if (sfxStartGame != null && sfxStartGame.IsValid())
            sfxStartGame.Post(gameObject);

        StartCoroutine(LoadSceneAsyncRoutine("Scene_Nightime"));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        _isLoading = true;

        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        yield return null;

        float elapsedTime = 0.0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float loadProgress = Mathf.Clamp01(operation.progress / 0.9f);
            float timeProgress = minLoadTime > 0.0f ? Mathf.Clamp01(elapsedTime / minLoadTime) : 1.0f;

            float displayProgress = Mathf.Min(loadProgress, timeProgress);

            if (progressBar != null)
                progressBar.value = displayProgress;

            if (progressText != null)
                progressText.text = $"{Mathf.RoundToInt(displayProgress * 100f)}%";

            if (operation.progress >= 0.9f && elapsedTime >= minLoadTime)
            {
                if (progressBar != null) 
                    progressBar.value = 1.0f;
                if (progressText != null)
                    progressText.text = "100%";

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ReturnToMainMenu(string mainMenuSceneName)
    {
        ServiceProvider.Instance.GetService<EventBus>()?.Clear();
        
        if (_isLoading)
            return;

        if (manager != null)
            manager.ClearStack();
        
        Time.timeScale = 1.0f;
        AkUnitySoundEngine.StopAll();

        StartCoroutine(LoadSceneAsyncRoutine("MainMenu"));
    }
}