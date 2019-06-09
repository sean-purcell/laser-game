using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // FIXME this is too small, beam logic shouldn't care
    public const float MAX_UPDATE_TIME = 0.004f;

    // FIXME dont default this
    private static string levelName = "dev";

    public static void OpenLevel(string name) {
        levelName = name;
        SceneManager.LoadScene("PuzzleBase");
    }

    public BeamHandler beamPrefab;
    public GameObject beamParent;

    public PuzzleHandler puzzle;
    public GameObject tileParent;

    public Transform camera;

    public bool playing;

    public float simTime;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");

        InitPuzzle();
        InitTiles();
        InitBeamParent();

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

    public BeamHandler CreateBeam(Vector3 start, Vector3 dir)
    {
        var beam = Instantiate<BeamHandler>(beamPrefab, beamParent.transform);
        beam.InitBeam(this, start, dir);
        return beam;
    }

    // Update is called once per frame
    void Update()
    {
        // Assume time only advances for now
        if (playing) {
            int updates = Mathf.CeilToInt(Time.deltaTime / MAX_UPDATE_TIME);
            float dt = Time.deltaTime / updates;
            for (int i = 0; i < updates; i++) {
                simTime += dt;
                DoProcess(dt);
            }
        }
    }

    private void DoProcess(float dt)
    {
        foreach (var beam in GetBeams()) {
            beam.Process(dt);
        }
        foreach (var tile in GetTiles()) {
            tile.Process(dt);
        }
    }

    private void InitPuzzle()
    {
        puzzle = GameObject.Find("/Puzzle").GetComponent<PuzzleHandler>();
        puzzle.Init();
    }

    private void InitTiles()
    {
        tileParent = GameObject.Find("/Tiles");
        foreach (var tile in GetTiles()) {
            tile.Init(this);
        }
    }

    private void InitBeamParent()
    {
        beamParent = GameObject.Find("/Beams");
        if (beamParent == null) {
            beamParent = new GameObject();
            beamParent.name = "Beams";
        }
    }

    private TileHandler[] GetTiles()
    {
        return tileParent.GetComponentsInChildren<TileHandler>();
    }

    private BeamHandler[] GetBeams()
    {
        return beamParent.GetComponentsInChildren<BeamHandler>();
    }
}
