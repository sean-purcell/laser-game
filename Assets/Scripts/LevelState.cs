using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LevelState
{
    New,
    Visited,
    Completed
}

static class LevelStateMethods
{
    public static string Show(this LevelState state)
    {
        switch(state)
        {
            case LevelState.New:
                return "New";
            case LevelState.Visited:
                return "Visited";
            case LevelState.Completed:
                return "Completed";
            default:
                return "Show not implemented for state";
        }
    }

    public static LevelState FromString(string s)
    {
        if (s.Equals("New"))
        {
            return LevelState.New;
        }
        else if (s.Equals("Visited"))
        {
            return LevelState.Visited;
        }
        else if (s.Equals("Completed"))
        {
            return LevelState.Completed;
        }
        // Debug.Warn("LevelState::FromString received weird string: " + s);
        return LevelState.New;
    }
}
