using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHandler : MonoBehaviour
{
    const float SPEED = 5;
    public GameHandler game;

    public LineRenderer r;
    public Vector3 start;
    public Vector3 end; // current head of wave, propagates
    public Vector3 dir;
    public bool propagating;

    public void InitBeam(GameHandler h, Vector3 start, Vector3 dir) {
        this.game = h;
        this.start = start;
        this.end = start;
        this.dir = Vector3.Normalize(dir);
        this.propagating = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        r.SetPosition(0, start);
    }

    // Update is called once per frame
    void Update()
    {
        if (propagating)
        {
            RaycastHit hit;
            if (Physics.Raycast(end, dir, out hit) && (hit.distance < SPEED * Time.deltaTime))
            {
                Debug.Log("collision");
                end = hit.point;
                hit.transform.gameObject.gameObject.GetComponent<TileHandler>().OnBeamCollision(this, hit);
            }
            if (!propagating) {
                end = hit.point;
            } else {
                end += SPEED * Time.deltaTime * dir;
            }
            r.SetPosition(1, end);
        }
    }
}
