using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerHandler : MonoBehaviour
{
    new public Camera camera;

    public float MAX_DIST = 100.0f;

    public float holdDistance = 10;

    public bool TeleportOn = true;

    public Collider floor;

    public FPDragHandler carrying = null;

    int dragLayerMask;

    void Start() {
        dragLayerMask =
            1 << LayerMask.NameToLayer("Glass") |
            1 << LayerMask.NameToLayer("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckMouseDown()) {
            MouseDown();
        }
        if (CheckMouseUp()) {
            MouseUp();
        }
    }

    void FixedUpdate()
    {
        if (carrying != null) {
            UpdateCarrying();
        }
    }

    public bool IsCarrying()
    {
        return carrying != null;
    }

    private bool CheckMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) {
            return true;
        }
        if (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began) {
            return true;
        }
        return false;
    }

    private bool CheckMouseUp()
    {
        if (Input.GetMouseButtonUp(0)) {
            return true;
        }
        if (Input.touchCount > 0 && (
                Input.GetTouch(0).phase == TouchPhase.Ended || 
                Input.GetTouch(0).phase == TouchPhase.Canceled)) {
            return true;
        }
        return false;
    }

    private void MouseDown()
    {
        // Find out what got hit
        Debug.Log("Mouse down, pos: " + GetPos() + ", dir: " + GetDir(), this);
        if (!Physics.Raycast(GetPos(), GetDir(), out RaycastHit hit, MAX_DIST))
            return;
        Collider collider = hit.collider;
        Debug.Log("Click collided with " + collider, this);

        if (collider == floor) {
            if (TeleportOn)
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

    protected abstract void Teleport(RaycastHit hit);

    private bool GetDragHandler(Collider c, out FPDragHandler dh)
    {
        dh = c.GetComponentInParent<FPDragHandler>();
        return dh != null;
    }

    private void PickUpObject(FPDragHandler dh)
    {
        if (dh.enabled) {
            carrying = dh;
            carrying.SetTarget(carrying.transform.position);
            carrying.dragging = true;
        }
    }

    private void UpdateCarrying()
    {
        // Cast a ray to see if we should move the block
        var ray = new Ray(
            GetPos() - carrying.transform.position.y * Vector3.up,
            GetDir());
        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                holdDistance,
                dragLayerMask)) {
            if (hit.collider == floor) {
                carrying.SetTarget(hit.point +
                        carrying.transform.position.y * Vector3.up);
            }
        }
    }

    public abstract Vector3 GetPos();

    public abstract Vector3 GetDir();
}
