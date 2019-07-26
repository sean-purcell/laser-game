using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // FIXME this is too small, beam logic shouldn't care
    public const float MAX_UPDATE_TIME = 0.004f;

    public BeamHandler beamPrefab;
    public GameObject beamParent;

    public PuzzleHandler puzzle;
    public GameObject tileParent;

    public Transform camera;

    public bool playing;

    public float simTime;

    public Action cleanup;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");

        InitPuzzle();
        InitTiles();
        InitBeamParent();

        simTime = 0;

        cleanup = null;
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
        ProcessInput();
        if (puzzle.IsWin()){
            puzzle.UpdateWinTargets();

            // TODO: "Next puzzle" button should appear on the navigation menu
        }
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown("space")) {
            playing = !playing;
        }
        if (Input.GetKeyDown("q")) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void FixedUpdate()
    {
        // Assume time only advances for now
        if (IsPlaying()) {
            simTime += Time.fixedDeltaTime;
            DoProcess(Time.fixedDeltaTime);
        }
    }

    private bool IsPlaying()
    {
        // For now only pause when carrying an item
        foreach (var player in GameObject.Find("/PuzzleBase/Player")
                    .GetComponentsInChildren<PlayerHandler>()) {
            if (player.IsCarrying()) {
                return false;
            }
        }
        return true;
    }

    private void DoProcess(float dt)
    {
        cleanup = null;
        foreach (var tile in GetTiles()) {
            tile.Process(dt);
        }
        foreach (var beam in GetBeams()) {
            beam.Process(dt);
        }
        if (cleanup != null) {
            cleanup();
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
