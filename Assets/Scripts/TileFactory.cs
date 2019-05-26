using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public FlatMirrorHandler flatMirrorPrefab;

    public FlatMirrorHandler CreateFlatMirror()
    {
        return (FlatMirrorHandler) Instantiate<FlatMirrorHandler>(flatMirrorPrefab);
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
