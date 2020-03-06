using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButtonHandler : MonoBehaviour
{
    public Text levelLabel;

    public Image levelStateCompletedIcon;

    // Maybe this should have some sensible default value? These need to be
    // unique.
    private string level;

    // Initializes the button with the appropriate level.
    public void Setup(string level)
    {
        this.level = level;
        levelLabel.text = level;

        this.hideAllIcons();

        switch (PersistenceHandler.GetLevelState(level))
        {
            case LevelState.New:
                break;
            case LevelState.Visited:
                // TODO 
                break;
            case LevelState.Completed:
                levelStateCompletedIcon.enabled = true;
                break;
            default:
                break;
        }
    }

    /**
     * Loads the level that the button has been initialized with.
     * Setup() must have been called already.
     */
    public void LoadLevel()
    {
        SceneManager.LoadScene(level);
    }

    private void hideAllIcons()
    {
        levelStateCompletedIcon.enabled = false;
    }
}
