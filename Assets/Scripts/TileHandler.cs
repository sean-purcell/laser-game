using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TileHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called once per beam, usually.
    abstract public void OnBeamCollision(BeamHandler beam, RaycastHit hit);
}
