using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : TileHandler
{
    // TODO: this approach doesnt support dragging, if needed modify this
    public TargetHandler target;

    public Vector2 endOffset;
    // Public to allow starting at non-base
    public Vector2 currentOffset;
    public float speed = 1.0f;

    private Vector3 basePos;

    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.position;
    }

    void Update()
    {
        transform.position = basePos + (Vector3) currentOffset;
    }

    public override void Process(float dt)
    {
        Vector2 dest = Vector2.zero;
        if (target.IsActive()) {
            dest = endOffset;
        }
        Vector2 diff = dest - currentOffset;
        if (diff.magnitude > 1e-8) {
            Vector2 update = diff.normalized *
                Mathf.Min(diff.magnitude, speed * dt);
            currentOffset += update;
        }
    }

    public override List<BeamHandler> OnBeamCollision(BeamHandler beam, RaycastHit hit)
    {
        return null;
    }
}
