using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TileHandler : MonoBehaviour
{
    protected GameHandler game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(GameHandler game, (int row, int col) tile, float orientation)
    {
        this.game = game;
        transform.localPosition = new Vector2(tile.col + 0.5f, tile.row + 0.5f);
        transform.Rotate(0, 0, orientation);
    }

    // Called once per beam, usually.
    abstract public void OnBeamCollision(BeamHandler beam, RaycastHit hit);
}
