using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpPlayerHandler : PlayerHandler
{
    protected override void Teleport(RaycastHit hit) {
        Vector3 npos = hit.point;
        npos.y = 2;
        transform.position = npos;
    }

    public override Vector3 GetPos()
    {
        return transform.position;
    }

    public override Vector3 GetDir()
    {
        return camera.transform.TransformDirection(Vector3.forward);
    }
}
