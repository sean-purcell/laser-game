using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    public int left;
    public int right;
    public int bottom;
    public int top;

    public Transform wallPrefab;

    public List<TargetHandler> winTargets;

    private const int WALL_SIZE = 500;

    // Start is called before the first frame update
    public void Init()
    {
        /*
        // Instantiate the walls

        var tileParent = GameObject.Find("/Tiles");
        var camera = Camera.main;

        List<Vector2> wallPositions = new List<Vector2> {
            new Vector2(left - WALL_SIZE / 2, 0),
            new Vector2(right + WALL_SIZE / 2, 0),
            new Vector2(0, bottom - WALL_SIZE / 2),
            new Vector2(0, top + WALL_SIZE / 2),
        };

        foreach (var pos in wallPositions) {
            Transform wall = Instantiate<Transform>(wallPrefab, tileParent.transform);
            wall.position = pos;
            wall.localScale = new Vector3(WALL_SIZE, WALL_SIZE, 1);
        }


        camera.transform.position = new Vector3(
                (left + right) / 2.0f,
                (top + bottom) / 2.0f,
                camera.transform.position.z);
                */
    }

    public bool IsWin()
    {
        return winTargets.All(target => target.IsActive());
    }

    public void UpdateWinTargets()
    {
        foreach (TargetHandler target in winTargets) {
            target.ShowWin();
        }
    }
}
