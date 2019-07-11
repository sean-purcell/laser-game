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

    public float start;
    public float end;

    public bool powered;

    public TileHandler endPoint;
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public List<BeamHandler> children;

    int layerMask;

    public void InitBeam(GameHandler h, Vector3 start, Vector3 dir)
    {
        this.game = h;

        transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        transform.position = start;

        this.start = 0;
        this.end = 0;

        this.powered = true;

        this.endPoint = null;
        this.children = null;

        layerMask = 1 << LayerMask.NameToLayer("Tile");

        SetEndpoints();
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
    public void Process(float dt)
    {
        // For now assume that dt > 0
        float time = game.SimTime();

        float dl = dt * SPEED;
        RaycastHit hit;
        bool res = Physics.Raycast(
                    GetPoint(start),
                    GetDir(),
                    out hit,
                    end - start + dl,
                    layerMask);

        HandleCollision(dt, start, end, res, hit);
        if (res) {
            end = start + hit.distance;
        } else {
            end += dl;
        }
        if (!powered) {
            start += dl;
        }

        if (start >= end) {
            DepowerChildren();
            Debug.Log("Beam self-destroying", this);
            GameObject.Destroy(gameObject);
            return;
        }

        SetEndpoints();
    }

    private void HandleCollision(float dt, float start, float end, bool hasHit, RaycastHit hit)
    {
        TileHandler tile = null;
        if (hasHit) {
            tile = hit
                .collider
                .gameObject
                .GetComponentInParent<TileHandler>();
        }

        bool hitMatches = hasHit && endPoint == tile &&
                ApproxEqual(hit.point, hitPoint) &&
                ApproxEqual(hit.normal, hitNormal);
        if (children != null && (!hasHit || !hitMatches)) {
            // If we have a real hit that doesn't exist anymore (because the
            // source moved), stop powering the children.

            Debug.Log("Hit lost " + hasHit + " " + hitMatches, this);
            DepowerChildren();
            endPoint = null;
            children = null;
        }

        // If we don't have a hit theres nothing to do
        if (!hasHit) {
            return;
        }

        if (hitMatches) {
            // We've already handled this collision
            return;
        }

        // If we didn't hit this just in the new segment of the ray
        if (hit.distance < end - start) {
            float newStart = start + hit.distance + FindOtherSide(hit);
            if (newStart <= end - 0.01) {
                // New ray!
                var dir = GetDir();
                var beam = game.CreateBeam(transform.position + newStart * dir, dir);

                beam.end = end - newStart;

                beam.powered = false;

                beam.endPoint = endPoint;
                beam.children = children;

                beam.SetEndpoints();
            }
        }

        children = null;
        endPoint = tile;
        hitPoint = hit.point;
        hitNormal = hit.normal;
        children = tile.OnBeamCollision(this, hit);
        if (children == null) {
            children = new List<BeamHandler>();
        }
    }

    private bool ApproxEqual(Vector3 a, Vector3 b, float prec=1e-3f)
    {
        return (a-b).sqrMagnitude <= prec*prec;
    }

    private void DepowerChildren()
    {
        if (children == null)
            return;
        foreach (var beam in children) {
            game.cleanup += delegate () {
                beam.powered = false;
            };
        }
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

        hit.collider.Raycast(new Ray(start + dir * dist, -dir), out hit2, dist);
        // Assert.AreEqual(hit.collider, hit2.collider); // vacuously true

        return 1000 - hit2.distance - EPS;
    }

    private void SetEndpoints()
    {
        renderer.SetPosition(0, start * Vector3.right);
        renderer.SetPosition(1, end * Vector3.right);

        collider.center = (end + start) / 2 * Vector3.right;
        collider.height = end - start;

        bool enabled = (end - start) > 0;
        renderer.enabled = enabled;
        collider.enabled = enabled;
    }

    public Vector3 GetPoint(float param = 0)
    {
        return transform.position + param * GetDir();
    }

    public Vector3 GetDir()
    {
        return transform.TransformDirection(1, 0, 0);
    }
}
