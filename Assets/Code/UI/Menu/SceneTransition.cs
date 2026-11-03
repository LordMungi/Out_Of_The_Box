using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public AK.Wwise.Event sfxStartGame; 

    public void StartGame(string gameplaySceneName)
    {
        if (sfxStartGame != null)
            sfxStartGame.Post(gameObject);

        SceneManager.LoadScene("Scene_Nightime");
    }

    public void ReturnToMainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1.0f;

        AkUnitySoundEngine.StopAll();

        SceneManager.LoadScene("MainMenu");
    }
}