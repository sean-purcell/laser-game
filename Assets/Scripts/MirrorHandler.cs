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

    public override List<BeamHandler> OnBeamCollision(BeamHandler beam, RaycastHit hit)
    {
        Vector3 reflectedDir = Vector3.Reflect(beam.GetDir(), hit.normal);
        var beams = new List<BeamHandler>();
        beams.Add(game.CreateBeam(hit.point, reflectedDir, beam));
        return beams;
    }
}
