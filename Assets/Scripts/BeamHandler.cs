using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHandler : MonoBehaviour
{
    const float SPEED = 5;

    public GameHandler game;

    public LineRenderer renderer;
    public CapsuleCollider collider;

    public bool propagating;

    public float startTime;
    public float poweredUntil;

    public TileHandler endPoint;
    public List<BeamHandler> children;

    int layerMask;

    public void InitBeam(GameHandler h, Vector3 start, Vector3 dir)
    {
        this.game = h;

        float ang = Mathf.Atan2(dir.y, dir.x);
        transform.localEulerAngles = new Vector3(0, 0, ang * Mathf.Rad2Deg);
        transform.position = start;

        this.propagating = true;

        this.startTime = h.SimTime();
        this.poweredUntil = Mathf.Infinity;

        this.endPoint = null;
        this.children = null;

        layerMask = 1 << LayerMask.NameToLayer("Tile");

        SetEndpoints(0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Use custom update called by GameHandler
    public void Process()
    {
        float time = game.SimTime();
        if (time < startTime) {
            GameObject.Destroy(gameObject);
            return;
        }

        float end = (time - startTime) * SPEED;
        float start = 0;
        if (time > poweredUntil) {
            start = (time - poweredUntil) * SPEED;
        }

        RaycastHit hit;
        if (Physics.Raycast(
                    transform.position,
                    GetDir(),
                    out hit,
                    end,
                    layerMask)) {
            HandleCollision(end, hit);
            end = hit.distance;
        }
        SetEndpoints(start, end);
    }

    private void HandleCollision(float end, RaycastHit hit)
    {
        var tile = hit
            .collider
            .gameObject
            .GetComponentInParent<TileHandler>();
        if (tile == endPoint) {
            // We've already handled this collision
            return;
        }
        endPoint = tile;
        children = tile.OnBeamCollision(this, hit);
        if (children == null) {
            children = new List<BeamHandler>();
        }
    }

    private void SetEndpoints(float start, float end)
    {
        renderer.SetPosition(0, start * Vector3.right);
        renderer.SetPosition(1, end * Vector3.right);

        collider.center = (end + start) / 2 * Vector3.right;
        collider.height = end - start;

        bool enabled = (end - start) > 1e-4;
        renderer.enabled = enabled;
        collider.enabled = enabled;
    }

    public Vector3 GetDir()
    {
        return transform.TransformDirection(1, 0, 0);
    }
}
