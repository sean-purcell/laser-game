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

    public const float SHRINK_FACTOR = 0.8f;

    public bool shrinkOnDrag = true;

    private Transform rendererChild;
    private Vector3 originalScale;
    private int originalLayer;

    private int draggingLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if (shrinkOnDrag) {
            rendererChild = transform.Find("Renderer");
        }
        draggingLayer = LayerMask.NameToLayer("Dragging");
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

    public void StartDrag()
    {
        dragging = true;

        originalLayer = gameObject.layer;
        gameObject.layer = draggingLayer;
        if (rendererChild != null) {
            originalScale = rendererChild.localScale;
            rendererChild.localScale = originalScale * SHRINK_FACTOR;
        }
    }

    public void StopDrag()
    {
        dragging = false;

        gameObject.layer = originalLayer;
        if (rendererChild != null) {
            rendererChild.localScale = originalScale;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        // Only implementing this interface to get marked as interactive by GVR
    }
}
