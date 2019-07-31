using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public string newGameScene;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayMode.overrideMode = true;
        if (SceneManager.GetActiveScene().name.Contains("Vr")) {
            PlayMode.modeOverride = PlayMode.Mode.Vr;
        } else {
            PlayMode.modeOverride = PlayMode.Mode.Topdown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // TODO: Do this more cleanly
    public void LoadVrMenu()
    {
        PlayMode.overrideMode = false;
        SceneManager.LoadScene("MainMenuVr");
    }

    public void LoadTdMenu()
    {
        PlayMode.overrideMode = false;
        SceneManager.LoadScene("MainMenu");
    }
}
