using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerHandler : MonoBehaviour
{
    new public Camera camera;

    public float MAX_DIST = 100.0f;

    public float holdDistance = 10;

    public enum TeleportMode {
        Off = 0,
        Anywhere = 1,
        Beacon = 2,
    };
    public TeleportMode teleportMode;

    public BeaconHandler currentBeacon;

    public Collider floor;

    public FPDragHandler carrying = null;

    int clickLayerMask;
    int dragLayerMask;

    void Start() {
        clickLayerMask =
            1 << LayerMask.NameToLayer("Glass") |
            1 << LayerMask.NameToLayer("Wall") |
            1 << LayerMask.NameToLayer("Tile") |
            1 << LayerMask.NameToLayer("Beacon");
        dragLayerMask =
            1 << LayerMask.NameToLayer("Glass") |
            1 << LayerMask.NameToLayer("Wall");
        if (currentBeacon != null) {
            Teleport(currentBeacon);
        }
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
        if (!Physics.Raycast(
                    GetPos(),
                    GetDir(),
                    out RaycastHit hit,
                    MAX_DIST,
                    clickLayerMask))
            return;
        Collider collider = hit.collider;
        Debug.Log("Click collided with " + collider, this);

        if (collider == floor) {
            if (teleportMode == TeleportMode.Anywhere)
                Teleport(hit);
        } else if (GetHandler<FPDragHandler>(collider, out FPDragHandler dh)) {
            PickUpObject(dh);
        } else if (GetHandler<BeaconHandler>(collider, out BeaconHandler bh)) {
            if (teleportMode == TeleportMode.Beacon) {
                Teleport(bh);
            }
        }
    }

    private void MouseUp()
    {
        // Leave the object wherever it was
        if (carrying != null) {
            carrying.StopDrag();
            carrying = null;
        }
    }

    private void Teleport(RaycastHit hit)
    {
        Vector3 npos = hit.point;
        npos.y = transform.position.y;
        transform.position = npos;
    }

    private void Teleport(BeaconHandler bh)
    {
        if (currentBeacon != bh && currentBeacon != null) {
            currentBeacon.Leave();
        }

        Vector3 npos = bh.transform.position;
        npos.y = transform.position.y;
        transform.position = npos;

        currentBeacon = bh;
        currentBeacon.Enter();
    }

    private bool GetHandler<T>(Collider c, out T dh)
    {
        dh = c.GetComponentInParent<T>();
        return dh != null;
    }

    private void PickUpObject(FPDragHandler dh)
    {
        if (dh.enabled) {
            carrying = dh;
            carrying.StartDrag();
            carrying.SetTarget(carrying.transform.position);
        }
    }

    private void UpdateCarrying()
    {
        // Determine how far away the "ground" is, inefficiently
        var ray = new Ray(
            GetPos() - carrying.transform.position.y * Vector3.up,
            GetDir());
        if (!floor.Raycast(
                ray,
                out RaycastHit floorHit,
                holdDistance)) {
            return;
        }
        // Cast another ray from our actual positionto see if anything is in
        // the way
        if (!Physics.Raycast(
                GetPos(),
                GetDir(),
                floorHit.distance - 1e-5f,
                dragLayerMask)) {
            carrying.SetTarget(floorHit.point +
                    carrying.transform.position.y * Vector3.up);
        }
    }

    public abstract Vector3 GetPos();

    public abstract Vector3 GetDir();
}
