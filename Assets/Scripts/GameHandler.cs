using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    float simTime_;

    //public GameBoard board;

    public BeamHandler beamPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");
        //LevelBuilder.Build();
        BuildLevel();
        simTime_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        simTime_ += Time.deltaTime;
    }

    public float SimTime() {
        return simTime_;
    }


    void BuildLevel() {
        BeamHandler b = (BeamHandler) Instantiate<BeamHandler>(beamPrefab);
        b.InitBeam(this, new Vector2(0,0), new Vector2(1, 0));

    }
}
