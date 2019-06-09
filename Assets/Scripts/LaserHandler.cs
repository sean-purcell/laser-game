using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : TileHandler
{
    private BeamHandler beam;

    // Start is called before the first frame update
    void Start()
    {
        beam = game.CreateBeam(
            transform.TransformPoint(new Vector3(0, 0.5f, 0)),
            transform.TransformDirection(new Vector3(0, 1, 0))
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
