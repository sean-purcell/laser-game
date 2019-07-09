using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public float MAX_DIST = 100.0f;

    public float holdDistance = 10;

    public Collider floor;

    public FPDragHandler carrying = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            MouseDown();
        }
        if (carrying != null) {
            UpdateCarrying();
        }
        if (Input.GetMouseButtonUp(0)) {
            MouseUp();
        }
    }

    private void MouseDown()
    {
        // Find out what got hit
        if (!Physics.Raycast(GetPos(), GetDir(), out RaycastHit hit, MAX_DIST))
            return;
        Collider collider = hit.collider;
        Debug.Log("Click collided with " + collider, this);

        if (collider == floor) {
            Teleport(hit);
        } else if (GetDragHandler(collider, out FPDragHandler dh)) {
            PickUpObject(dh);
        }
    }

    private void MouseUp()
    {
        // Leave the object wherever it was
        // TODO: this will leave objects in midair
        if (carrying != null) {
            carrying.dragging = false;
            carrying = null;
        }
    }

    private void Teleport(RaycastHit hit) {
        Vector3 npos = hit.point;
        npos.y = 2;
        transform.position = npos;
    }

    private bool GetDragHandler(Collider c, out FPDragHandler dh)
    {
        dh = c.GetComponentInParent<FPDragHandler>();
        return dh != null;
    }

    private void PickUpObject(FPDragHandler dh)
    {
        if (dh.enabled) {
            carrying = dh;
            carrying.dragging = true;
        }
    }

    private void UpdateCarrying()
    {
        Vector3 targetPos = GetPos() + holdDistance * GetDir();
        // Cast a ray for the ground.  We're looking to place
        // the object 0.5 units above the ground.
        if (floor.Raycast(new Ray(GetPos() - 0.5f * Vector3.up, GetDir()),
                out RaycastHit hit,
                holdDistance)) {
            targetPos = hit.point + 0.5f * Vector3.up;
        }
        carrying.SetTarget(targetPos);
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public Vector3 GetDir()
    {
        return transform.TransformDirection(Vector3.forward);
    }
}
