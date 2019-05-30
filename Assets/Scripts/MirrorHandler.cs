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
        Vector3 reflectedDir = Vector3.Reflect(beam.GetDir(), hit.normal);
        var beams = GameObject.Find("/Beams");
        BeamHandler b = (BeamHandler)Instantiate<BeamHandler>(beam, beams.transform);
        b.InitBeam(beam.game, hit.point, reflectedDir);
    }
}
