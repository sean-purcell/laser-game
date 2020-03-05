using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PersistenceHandler
{
    public static LevelState GetLevelState(string levelName)
    {
        // TODO: gotta actually check the persistent storage
        return LevelState.New;
    }

    public static void SetLevelState(string levelName, LevelState state)
    {
        // TODO
        return;
    }
}
