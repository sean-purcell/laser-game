using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private Camera cam;

    private Vector3 scale;

    private Transform xform;
    private TileHandler tile;

    void Start()
    {
        cam = Camera.main;
        xform = transform.parent;
        tile = xform.gameObject.GetComponent<TileHandler>();
    }

    void OnMouseDown()
    {
        if (!enabled) return;
        Vector3 npos = xform.position;
        npos.z = -1;
        xform.position = npos;

        scale = xform.localScale;
        xform.localScale = scale * 0.8f;

        /*
        Material mat = GetComponent<Renderer>().material;
        Color ncol = mat.color;
        ncol.a = 0.5f;
        mat.color = ncol;
        */
    }

    void OnMouseUp()
    {
        if (!enabled) return;
        Vector3 npos = xform.position;
        npos.z = 0;
        xform.position = npos;

        xform.localScale = scale;

        /*
        Material mat = GetComponent<Renderer>().material;
        Color ncol = mat.color;
        ncol.a = 1;
        mat.color = ncol;
        */
    }

    void OnMouseDrag()
    {
        if (!enabled) return;
        Vector3 pos = Input.mousePosition;
        pos.z = -cam.transform.position.z;
        pos = cam.ScreenToWorldPoint(pos);

        float nx = Mathf.Floor(pos.x) + 0.5f;
        float ny = Mathf.Floor(pos.y) + 0.5f;

        Debug.Log("Attempting new position of " + nx + "," + ny);
        if (tile == null || tile.IsPositionValid(new Vector2(nx, ny))) {
            xform.position = new Vector3(nx, ny, xform.position.z);
        }
    }
}
