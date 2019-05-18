using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHandler : MonoBehaviour
{
    const float SPEED = 0.5f;
    public  GameHandler game;

    public LineRenderer r;
    public Vector2 start;
    public Vector2 dir;

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
