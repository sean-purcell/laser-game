﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandler : TileHandler
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
        beam.propagating = false;
    }
}