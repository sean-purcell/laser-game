using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : TileHandler
{
    public BeamHandler beamPrefab;

    // Start is called before the first frame update
    void Start()
    {
        var beams = GameObject.Find("/Beams");
        var beam = (BeamHandler)Instantiate<BeamHandler>(beamPrefab, beams.transform);
        beam.InitBeam(game,
                transform.TransformPoint(new Vector3(0, 0.5f, 0)),
                transform.TransformDirection(new Vector3(0, 1, 0))
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnBeamCollision(BeamHandler beam, RaycastHit hit)
    {
        beam.propagating = false;
    }
}
