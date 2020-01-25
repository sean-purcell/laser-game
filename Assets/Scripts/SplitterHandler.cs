using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterHandler : TileHandler
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

        // Low-intensity beams are destroyed on contact with a beam-splitter,
        // since both re-emissions are too dim to trigger anything.
        if (beam.intensity > 0) {
            BeamHandler continuation = game.CreateBeam(hit.point + 0.000001f * beam.GetDir(), beam.GetDir(), beam);
            BeamHandler reemission = game.CreateBeam(hit.point, reflectedDir, beam);

            continuation.intensity -= 1;
            reemission.intensity -= 1;

            beams.Add(continuation);
            beams.Add(reemission);
        }
        return beams;
    }
}
