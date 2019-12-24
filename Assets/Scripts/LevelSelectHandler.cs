using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* There are two steps to adding a level (scene) to this menu:
 * 1. In the MainMenu/Canvas/LevelsPanel, add the
      scene's name to the "Levels" field
 * 2. In File->Build Settings, add the new scene to "Scenes In Build"
 */

public class LevelSelectHandler : MonoBehaviour
{
    // Populate to programmatically add levels to the selection scrollview
    // during runtime.
    public List<string> levels;

    public GameObject levelSelectButtonPrefab;

    // This is where we'll instantiate the new level select buttons.
    public GameObject contentPanel;

    public Button prevPageButton;
    public Button nextPageButton;

    // Used for pagination. If the page is negative, we set it as 0 the next
    // time we populate. If it exceeds the last page, then we set it to the last
    // page.
    public int page;

    // Should also be positive.
    public int levelsPerPage;

    // Start is called before the first frame update
    void Start()
    {
        Populate();

        this.prevPageButton.onClick.AddListener(this.prevPage);
        this.nextPageButton.onClick.AddListener(this.nextPage);
    }

    private void prevPage()
    {
        --this.page;
        this.Populate();
    }

    private void nextPage()
    {
        ++this.page;
        this.Populate();
    }

    // Creates a button for each level.
    private void Populate()
    {
        // Keep the page count reasonable.
        int lastPage = (this.levels.Count - 1)/this.levelsPerPage;
        if (this.page < 0)
        {
            this.page = 0;
        }
        else if (this.page > lastPage)
        {
            this.page = lastPage;
        }

        foreach (Transform levelButton in contentPanel.transform)
        {
            GameObject.Destroy(levelButton.gameObject);
        }

        for (int ii = this.page * this.levelsPerPage;
             ii < (this.page + 1) * this.levelsPerPage && ii < this.levels.Count;
             ++ii)
        {
            var newButton = (GameObject)Instantiate(levelSelectButtonPrefab);
            newButton.transform.SetParent(contentPanel.transform, false);

            var buttonHandler = newButton.GetComponent<LevelSelectButtonHandler>();
            buttonHandler.Setup(this.levels[ii]);
        }
    }
}
