using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    // String should match name of the main menu scene.
    public string mainMenuScene;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
