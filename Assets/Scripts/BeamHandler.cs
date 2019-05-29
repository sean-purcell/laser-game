using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHandler : MonoBehaviour
{
    const float SPEED = 5;
    public GameHandler game;

    public LineRenderer r;
    public Vector3 start;
    public Vector3 dir;
    public bool propagating;

    public float startTime;

    public void InitBeam(GameHandler h, Vector3 start, Vector3 dir) {
        this.game = h;
        this.start = start;
        this.dir = Vector3.Normalize(dir);
        this.propagating = true;

        this.startTime = h.SimTime();
    }

    // Start is called before the first frame update
    void Start()
    {
        r.SetPosition(0, start);
    }

    // Update is called once per frame
    void Update()
    {
        float length = SPEED * (game.SimTime() - startTime);
        if (length < -1e-9)
        {
            Destroy(gameObject);
            return;
        }
        r.enabled = length > 1e-9;
        if (propagating)
        {
            RaycastHit hit;
            if (Physics.Raycast(start, dir, out hit) && (hit.distance <= length))
            {
                Debug.Log("collision");
                // TODO: add a parameter for the time at which the new beam should be generated
                hit.transform.gameObject.gameObject.GetComponent<TileHandler>().OnBeamCollision(this, hit);
            }
            Vector3 end = start + dir * length;
            if (!propagating) {
                end = hit.point;
            } else {
                end += SPEED * Time.deltaTime * dir;
            }
            r.SetPosition(1, end);
        }
    }
}
