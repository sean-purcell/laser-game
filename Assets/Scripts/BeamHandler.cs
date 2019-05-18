using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHandler : MonoBehaviour
{
    const float SPEED = 1;
    public  GameHandler game;

    public LineRenderer r;
    public Vector2 start;
    public Vector2 dir;

    public void InitBeam(GameHandler h, Vector2 start, Vector2 dir) {
        this.game = h;
        this.start = start;
        this.dir = dir;
    }

    // Start is called before the first frame update
    void Start()
    {
        r.SetPosition(0, start);
    }

    // Update is called once per frame
    void Update()
    {
        float len = SPEED * game.SimTime();
        r.SetPosition(1, len * dir + start);
        Debug.Log("Len: " + len.ToString());
    }
}
