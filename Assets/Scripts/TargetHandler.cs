using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : TileHandler
{

    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnBeamCollision(BeamHandler beam, RaycastHit hit)
    {
        renderer.material.color = Color.cyan;
        //game.Win();
    }
}
