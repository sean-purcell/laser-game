using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FPDragHandler : MonoBehaviour, IDragHandler
{
    private Rigidbody rb;

    public float maxSpeed = 10;

    public bool dragging;

    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate()
    {
        if (dragging) {
            rb.constraints &= ~RigidbodyConstraints.FreezePosition;

            Vector3 dist = target - transform.position;
            Vector3 npos = Vector3.MoveTowards(transform.position, target, 1);
            rb.velocity = (npos - transform.position) * maxSpeed;
        } else {
            rb.constraints |= RigidbodyConstraints.FreezePosition;
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void OnDrag(PointerEventData data)
    {
        // Only implementing this interface to get marked as interactive by GVR
    }
}
