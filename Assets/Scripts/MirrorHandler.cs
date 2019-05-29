using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorHandler : TileHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnBeamCollision(BeamHandler beam, RaycastHit hit)
    {
        // TODO: this is bunk and junk
        beam.propagating = false;
        Vector3 reflectedDir = Vector3.Reflect(beam.dir, hit.normal);
        BeamHandler b = (BeamHandler)Instantiate<BeamHandler>(beam);
        b.InitBeam(beam.game, beam.end, reflectedDir);
    }
}
