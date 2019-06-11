using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    public List<string> levels;
    public GameObject levelSelectButtonPrefab;
    public Transform contentPanel; // TODO(toyang): I wonder if we can get this from "this"

    // Start is called before the first frame update
    void Start()
    {
        // TODO(toyang): I think we might have to destroy this later, but not sure.
        Populate();
    }

    // TODO(toyang): I don't think we need this.
    // Update is called once per frame
    void Update()
    {
        
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
