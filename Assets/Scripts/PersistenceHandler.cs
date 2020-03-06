using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PersistenceHandler
{
    public static LevelState GetLevelState(string levelName)
    {
        string state = PlayerPrefs.GetString(levelName);
        return LevelStateMethods.FromString(state);
    }

    public static void SetLevelState(string levelName, LevelState state)
    {
        Debug.Log("Setting level " + levelName + " to state " + state.Show());
        PlayerPrefs.SetString(levelName, state.Show());
        return;
    }
}
