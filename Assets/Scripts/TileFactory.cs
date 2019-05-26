using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public FlatMirrorHandler flatMirrorPrefab;
    public LaserHandler laserPrefab;

    public FlatMirrorHandler CreateFlatMirror()
    {
        return (FlatMirrorHandler) Instantiate<FlatMirrorHandler>(flatMirrorPrefab);
    }

    public LaserHandler CreateLaser()
    {
        return (LaserHandler) Instantiate<LaserHandler>(laserPrefab);
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
