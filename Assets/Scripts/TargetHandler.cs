using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : TileHandler
{

    private Renderer renderer;

    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Beam");
        renderer = GetComponent<Renderer>();
        renderer.material.color = Color.grey;
    }

    void Update()
    {}

    public override void Process()
    {
        // Check for any collisions with lasers

        var collisions = Physics.OverlapSphere(
                transform.position,
                transform.localScale.x / 2,
                layerMask);
        foreach (var col in collisions) {
            Debug.Log("Hit target: " + col.gameObject.name);
        }
        if (collisions.Length > 0) {
            renderer.material.color = Color.cyan;
        } else {
            renderer.material.color = Color.grey;
        }
    }

    public override void OnBeamCollision(BeamHandler beam, RaycastHit hit)
    {
    }
}
