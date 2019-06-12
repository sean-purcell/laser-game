using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButtonHandler : MonoBehaviour
{
    public Text levelLabel;

    // Maybe this should have some sensible default value?
    private string level;

    // Initializes the button with the appropriate level.
    public void Setup(string level)
    {
        this.level = level;
        levelLabel.text = level;
    }

    /**
     * Loads the level that the button has been initialized with.
     * Setup() must have been called already.
     */
    public void LoadLevel()
    {
        SceneManager.LoadScene(level);
    }
}
