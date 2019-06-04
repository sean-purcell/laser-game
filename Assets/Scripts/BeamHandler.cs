using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BeamHandler : MonoBehaviour
{
    // FIXME: we should track start and endpoints, do this with delta-t
    const float SPEED = 5;

    private const float EPS = 1e-4f;

    public GameHandler game;

    public LineRenderer renderer;
    public CapsuleCollider collider;

    public float startTime;
    public float poweredUntil;

    public TileHandler endPoint;
    public float hitDist;
    public List<BeamHandler> children;

    int layerMask;

    public void InitBeam(GameHandler h, Vector3 start, Vector3 dir)
    {
        this.game = h;

        float ang = Mathf.Atan2(dir.y, dir.x);
        transform.localEulerAngles = new Vector3(0, 0, ang * Mathf.Rad2Deg);
        transform.position = start;

        this.startTime = h.SimTime();
        this.poweredUntil = Mathf.Infinity;

        this.endPoint = null;
        this.hitDist = Mathf.Infinity;
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
        bool res = Physics.Raycast(
                    transform.position,
                    GetDir(),
                    out hit,
                    end,
                    layerMask);

        HandleCollision(start, end, res, hit);
        if (res) {
            end = hit.distance;
        }

        start = Mathf.Min(start, end);

        SetEndpoints(start, end);
    }

    private void HandleCollision(float start, float end, bool hasHit, RaycastHit hit)
    {
        // Are we now shorter than a pre-existing hit?
        if (endPoint != null && end < hitDist) {
            // Delete old hit
            endPoint = null;
            hitDist = Mathf.Infinity;
            // The children will clean themselves up based on startTime
            children = null;
        }

        TileHandler tile = null;
        if (hasHit) {
            tile = hit
                .collider
                .gameObject
                .GetComponentInParent<TileHandler>();
        }

        if (!hasHit || tile != endPoint) {
            // If we have a real hit that doesn't exist anymore (because the
            // source moved), stop powering the children.

            if (children != null) {
                foreach (var beam in children) {
                    beam.SetPoweredUntil(game.SimTime());
                }
                endPoint = null;
                hitDist = Mathf.Infinity;
                children = null;
            }
        }

        // If we don't have a hit theres nothing to do
        if (!hasHit) {
            return;
        }

        if (tile == endPoint) {
            // We've already handled this collision
            return;
        }

        // FIXME: 
        float newStart = FindOtherSide(hit) + hit.distance;
        print(newStart + " + " + end);
        if (newStart <= end - 0.01) {
            // New ray!
            var dir = GetDir();
            var beam = game.CreateBeam(transform.position + newStart * dir, dir);

            beam.startTime = (end - newStart) / SPEED;
            beam.poweredUntil = game.SimTime() +
                Mathf.Max(0, start - newStart) / SPEED;

            beam.endPoint = endPoint;
            beam.children = children;
        }

        children = null;
        endPoint = tile;
        hitDist = hit.distance;
        // If we've already passed the collision point don't spawn children
        if (start < hit.distance - EPS) {
            children = tile.OnBeamCollision(this, hit);
        }
        if (children == null) {
            children = new List<BeamHandler>();
        }

        UpdateChildrenPowered();
    }

    private void UpdateChildrenPowered() {
        if (children == null) return;
        if (poweredUntil == Mathf.Infinity) return;

        float newTime = hitDist / SPEED + poweredUntil;
        foreach (var beam in children) {
            beam.SetPoweredUntil(newTime);
        }
    }

    public void SetPoweredUntil(float until) {
        poweredUntil = until;
        UpdateChildrenPowered();
    }

    private float FindOtherSide(RaycastHit hit)
    {
        // NB: This does not work with concave targets, if we create any of
        // those we need to fix this.  I spent some time trying to come up with
        // a solution for those and got nowhere other than advancing a point
        // until its outside the mesh, but that is slow and not worth doing for
        // now.

        var dir = GetDir();
        var start = hit.point + dir * EPS;

        float dist = 1000;
        RaycastHit hit2;

        Assert.IsTrue(hit.collider.Raycast(new Ray(start + dir * dist, -dir), out hit2, dist));
        // Assert.AreEqual(hit.collider, hit2.collider); // vacuously true

        return 1000 - hit2.distance - EPS;
    }

    private void SetEndpoints(float start, float end)
    {
        renderer.SetPosition(0, start * Vector3.right);
        renderer.SetPosition(1, end * Vector3.right);

        collider.center = (end + start) / 2 * Vector3.right;
        collider.height = end - start;

        bool enabled = (end - start) > EPS;
        renderer.enabled = enabled;
        collider.enabled = enabled;
    }

    public Vector3 GetDir()
    {
        return transform.TransformDirection(1, 0, 0);
    }
}
