﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    // Populate to programmatically add levels to the selection scrollview
    // during runtime.
    public List<string> levels;

    public GameObject levelSelectButtonPrefab;

    // This is where we'll instantiate the new level select buttons.
    public Transform contentPanel;

    // Start is called before the first frame update
    void Start()
    {
        // TODO(toyang): I think we might have to destroy this later, but not sure.
        Populate();
    }

    // Creates a button for each level. 
    private void Populate()
    {
        foreach (string level in levels)
        {
            var newButton = (GameObject)Instantiate(levelSelectButtonPrefab);
            newButton.transform.SetParent(contentPanel);

            var buttonHandler = newButton.GetComponent<LevelSelectButtonHandler>(); 
            buttonHandler.Setup(level);
        }
    }
}
