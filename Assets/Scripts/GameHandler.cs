using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameHandler : MonoBehaviour
{
    // FIXME this is too small, beam logic shouldn't care
    public const float MAX_UPDATE_TIME = 0.004f;

    public BeamHandler beamPrefab;
    public GameObject beamParent;

    public PuzzleHandler puzzle;
    public GameObject tileParent;

    public SprayEffectHandler sprayEffectPrefab;
    public GameObject sprayEffectParent;

    public PlayerHandler player;

    public string mainMenuScene;

    public Canvas winScreenCanvas;

    private bool won;
    private bool teleportedBack;

    private Vector3 oldPosition;

    private Vector3 oldEulerAngles;

    private Vector3 oldCameraEulerAngles;

    public bool playing;

    public float simTime;

    public Action cleanup;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");

        var scene = SceneManager.GetActiveScene();
        AnalyticsEvent.LevelStart(scene.name, scene.buildIndex);

        InitPuzzle();
        InitTiles();
        InitBeamParent();
        InitSprayEffectParent();

        simTime = 0;

        cleanup = null;

        won = false;
        teleportedBack = false;
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
    
    public BeamHandler CreateBeam(Vector3 start, Vector3 dir, BeamHandler template)
    {
        var beam = Instantiate<BeamHandler>(beamPrefab, beamParent.transform);
        beam.InitBeam(this, start, dir, template);
        return beam;
    }

    public SprayEffectHandler CreateSprayEffect()
    {
        return Instantiate<SprayEffectHandler>(sprayEffectPrefab, sprayEffectParent.transform);
    }
    
    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        if (puzzle.IsWin() && !won && !teleportedBack){
            puzzle.UpdateWinTargets();

            var scene = SceneManager.GetActiveScene();
            Analytics.CustomEvent("levelWin", new Dictionary<String, object>{
                { "level", scene.name },
                { "time", simTime },
            });


            won = true;

            PersistenceHandler.SetLevelState(SceneManager.GetActiveScene().name,
                                             LevelState.Completed);

            oldPosition = player.transform.position;
            oldEulerAngles = player.transform.eulerAngles;
            oldCameraEulerAngles = player.camera.transform.eulerAngles;

            // teleport to win screen and make it face us
            // NOTE: google VR stops us from rotating the user's camera, so we
            // rotate and shift the canvas to face the user.
            player.transform.position =
                new Vector3(winScreenCanvas.transform.position.x, 0, 0);

            // NOTE: the distance we set is based on the z coordinate of the win screen canvas.
            winScreenCanvas.transform.position =
                Camera.main.transform.position +
                Camera.main.transform.forward * winScreenCanvas.transform.position.z;
            // We can't just do Camera.main.transform.position here because we
            // end up looking at the back of the win screen.
            winScreenCanvas.transform.LookAt(2 * winScreenCanvas.transform.position -
                                             Camera.main.transform.position);
        }
    }

    public void TeleportBack()
    {
        Debug.Assert(won == true);
        teleportedBack = true;
        player.transform.position = oldPosition;
        player.transform.eulerAngles = oldEulerAngles;
        player.camera.transform.eulerAngles = oldCameraEulerAngles;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ToNextLevel()
    {
        var scene = SceneManager.GetActiveScene().name;
        var idx = int.Parse(scene.Substring(6));
        SceneManager.LoadScene("Puzzle" + (idx + 1));
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown("space")) {
            playing = !playing;
        }
        if (Input.GetKeyDown("q")) {
            SceneManager.LoadScene(mainMenuScene);
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
        /*
        // For now only pause when carrying an item
        foreach (var player in GameObject.Find("/PuzzleBase/Player")
                    .GetComponentsInChildren<PlayerHandler>()) {
            if (player.IsCarrying()) {
                return false;
            }
        }
        */
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

    private void InitSprayEffectParent()
    {
        sprayEffectParent = GameObject.Find("/SprayEffects");
        if (sprayEffectParent == null)
        {
            sprayEffectParent = new GameObject();
            sprayEffectParent.name = "SprayEffects";
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
