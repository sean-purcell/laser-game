using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdPlayerHandler : PlayerHandler
{
    protected override void Teleport(RaycastHit hit) {
    }

    public override Vector3 GetPos()
    {
        return transform.position;
    }

    public override Vector3 GetDir()
    {
        Vector3 pos = Input.mousePosition;
        if (Input.touchCount > 0) {
            pos = Input.GetTouch(0).position;
        }
        pos.z = 1;
        pos = camera.ScreenToWorldPoint(pos);
        return pos - transform.position;
    }
}
