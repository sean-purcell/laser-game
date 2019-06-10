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

    // Called more than once per frame by GameHandler
    public virtual void Process(float dt)
    {
    }

    public void Init(GameHandler game)
    {
        this.game = game;
    }

    public void Init(GameHandler game, (int row, int col) tile, float orientation)
    {
        Init(game);
        transform.localPosition = new Vector2(tile.col + 0.5f, tile.row + 0.5f);
        transform.Rotate(0, 0, orientation);
    }

    public virtual List<BeamHandler> OnBeamCollision(BeamHandler beam, RaycastHit hit)
    {
        return null;
    }

    public virtual bool IsPositionValid(Vector2 pos)
    {
        int collisionLayers =
            (1 << LayerMask.NameToLayer("Tile")) |
            (1 << LayerMask.NameToLayer("Glass"));
        // Assumes this tile is a 1x1 box
        float size = 0.4999f;
        var overlap = Physics.OverlapBox(pos, new Vector3(size, size, size),
                Quaternion.identity, collisionLayers);
        return overlap.Length == 0;
    }
}
