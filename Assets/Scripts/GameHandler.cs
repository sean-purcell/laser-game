using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // FIXME dont default this
    private static string levelName = "dev";

    public static void OpenLevel(string name) {
        levelName = name;
        SceneManager.LoadScene("PuzzleBase");
    }

    public TileFactory factory;
    public Transform camera;

    float simTime_;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");
        InitTiles();
        //LoadMap();
        simTime_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        simTime_ += Time.deltaTime;
    }

    public float SimTime() {
        return simTime_;
    }

    private void InitTiles()
    {
        var tiles = GameObject
            .Find("/Tiles")
            .GetComponentsInChildren<TileHandler>();
        foreach (var tile in tiles) {
            tile.Init(this);
        }
    }

    private void LoadMap()
    {
        var map = MapParser.GetMap(@"Assets\Maps\" + levelName);

        CentreCamera(map.rows, map.cols);

        foreach (var entry in map.tiles) {
            TileHandler tile = ConstructTile(entry.Value);
            float orientation = GetOrientation(entry.Value);
            tile.Init(this, entry.Key, orientation);
        }
    }

    private void CentreCamera(int rows, int cols)
    {
        camera.position = new Vector3(cols / 2.0f, rows / 2.0f, -10);
    }

    private TileHandler ConstructTile(MapParser.TileType tile)
    {
        switch (tile)
        {
            case MapParser.TileType.MirrorForward:
            case MapParser.TileType.MirrorBackward:
                return factory.CreateFlatMirror();
            case MapParser.TileType.LaserLeft:
            case MapParser.TileType.LaserRight:
            case MapParser.TileType.LaserUp:
            case MapParser.TileType.LaserDown:
                return factory.CreateLaser();
            case MapParser.TileType.Wall:
                return factory.CreateWall();
            case MapParser.TileType.Target:
                return factory.CreateTarget();
            default:
                throw new Exception("Attempted to create unsupported tile type " + Enum.GetName(typeof(MapParser.TileType), tile));
        }
    }
    
    private float GetOrientation(MapParser.TileType tile)
    {
        switch (tile)
        {
            case MapParser.TileType.MirrorForward:
                return -45;
            case MapParser.TileType.MirrorBackward:
                return 45;
            case MapParser.TileType.LaserLeft:
                return 90;
            case MapParser.TileType.LaserRight:
                return -90;
            case MapParser.TileType.LaserUp:
                return 0;
            case MapParser.TileType.LaserDown:
                return 180;
            default:
                return 0;
        }
    }
}
