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

    int layerMask;

    public void InitBeam(GameHandler h, Vector3 start, Vector3 dir) {
        this.game = h;

        float ang = Mathf.Atan2(dir.y, dir.x);
        transform.localEulerAngles = new Vector3(0, 0, ang * Mathf.Rad2Deg);
        transform.position = start;

        this.propagating = true;

        this.startTime = h.SimTime();

        layerMask = 1 << LayerMask.NameToLayer("Tile");
    }

    // Start is called before the first frame update
    void Start()
    {
        renderer.SetPosition(0, Vector3.zero);
        renderer.SetPosition(1, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        const float EPS = 1e-4f;

        var time = game.SimTime();
        if (time < startTime - EPS) {
            Destroy(gameObject);
            return;
        }

        float length = SPEED * (game.SimTime() - startTime);
        if (length < -1e-9)
        {
            Destroy(gameObject);
            return;
        }
        renderer.enabled = length > 1e-9;
        collider.enabled = renderer.enabled;
        if (propagating)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, GetDir(), out hit, Mathf.Infinity, layerMask) &&
                    (hit.distance <= length))
            {
                Debug.Log("collision");
                // TODO: add a parameter for the time at which the new beam should be generated
                hit.transform.gameObject.gameObject.GetComponent<TileHandler>().OnBeamCollision(this, hit);
            }
            Vector3 end = new Vector3(length, 0, 0);
            if (!propagating) {
                end.x = hit.distance;
            }
            collider.center = new Vector3(end.x / 2, 0, 0);
            collider.height = end.x;
            renderer.SetPosition(1, end);
        }
    }

    public Vector3 GetDir()
    {
        return transform.TransformDirection(1, 0, 0);
    }
}
