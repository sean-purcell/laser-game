using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    float simTime_;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");
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
}
