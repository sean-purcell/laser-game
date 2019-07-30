using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpPlayerHandler : PlayerHandler
{
    public override Vector3 GetPos()
    {
        return transform.position;
    }

    public override Vector3 GetDir()
    {
        return camera.transform.TransformDirection(Vector3.forward);
    }
}
