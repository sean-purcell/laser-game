using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public MirrorHandler flatMirrorPrefab;
    public LaserHandler laserPrefab;
    public WallHandler wallPrefab;
    public TargetHandler targetPrefab;

    public MirrorHandler CreateFlatMirror()
    {
        return (MirrorHandler) Instantiate<MirrorHandler>(flatMirrorPrefab);
    }

    public LaserHandler CreateLaser()
    {
        return (LaserHandler) Instantiate<LaserHandler>(laserPrefab);
    }

    public WallHandler CreateWall()
    {
        return (WallHandler) Instantiate<WallHandler>(wallPrefab);
    }

    public TargetHandler CreateTarget()
    {
        return (TargetHandler) Instantiate<TargetHandler>(targetPrefab);
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
