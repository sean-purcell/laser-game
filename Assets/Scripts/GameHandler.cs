using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public const float MAX_UPDATE_TIME = 0.005f;

    // FIXME dont default this
    private static string levelName = "dev";

    public static void OpenLevel(string name) {
        levelName = name;
        SceneManager.LoadScene("PuzzleBase");
    }

    public TileFactory factory;
    public Transform camera;

    public bool playing;

    public float simTime;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");
        InitTiles();
        //LoadMap();
        playing = false;
        simTime = 0;
    }

    public void Play()
    {
        playing = true;
    }

    public void Pause()
    {
        playing = false;
    }

    public void Reset()
    {
        Pause();
        simTime = 0;
    }

    public float SimTime() {
        return simTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Assume time only advances for now
        if (playing) {
            int updates = Mathf.CeilToInt(Time.deltaTime / MAX_UPDATE_TIME);
            float otime = simTime;
            for (int i = 1; i <= updates; i++) {
                simTime = otime + i * Time.deltaTime / updates;
                DoProcess();
            }
        }
    }

    private void DoProcess()
    {
        foreach (var beam in GetBeams()) {
            beam.Process();
        }
        foreach (var tile in GetTiles()) {
            tile.Process();
        }
    }

    private void InitTiles()
    {
        foreach (var tile in GetTiles()) {
            tile.Init(this);
        }
    }

    private TileHandler[] GetTiles()
    {
        return GameObject
            .Find("/Tiles")
            .GetComponentsInChildren<TileHandler>();
    }

    private BeamHandler[] GetBeams()
    {
        return GameObject
            .Find("/Beams")
            .GetComponentsInChildren<BeamHandler>();
    }
}
