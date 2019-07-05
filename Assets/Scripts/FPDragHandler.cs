using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPDragHandler : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetTarget(Vector3 target)
    {
        rb.MovePosition(target);
    }
}
