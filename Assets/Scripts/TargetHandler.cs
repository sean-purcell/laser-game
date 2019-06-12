using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : TileHandler
{
    // Time the target must be active for before marking itself active
    public float cutoff = 0.5f;

    private Renderer renderer;

    private int layerMask;

    private float activeTime;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Beam");
        renderer = GetComponent<Renderer>();
        renderer.material.color = Color.grey;

        activeTime = 0;
    }

    void Update()
    {}

    public bool IsActive()
    {
        return activeTime >= cutoff;
    }

    public override void Process(float dt)
    {
        // Check for any collisions with lasers

        var collisions = Physics.OverlapSphere(
                transform.position,
                transform.localScale.x / 2,
                layerMask);
        if (collisions.Length > 0) {
            activeTime += dt;
        } else {
            activeTime = 0;
        }
        if (IsActive()) {
            renderer.material.color = Color.cyan;
        } else {
            renderer.material.color = Color.grey;
        }
    }
}
