using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private Camera cam;

    private Vector3 scale;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        if (!enabled) return;
        Vector3 npos = transform.position;
        npos.z = -1;
        transform.position = npos;

        scale = transform.localScale;
        transform.localScale = scale * 0.8f;

        Material mat = GetComponent<Renderer>().material;
        Color ncol = mat.color;
        ncol.a = 0.5f;
        mat.color = ncol;
    }

    void OnMouseUp()
    {
        if (!enabled) return;
        Vector3 npos = transform.position;
        npos.z = 0;
        transform.position = npos;

        transform.localScale = scale;

        Material mat = GetComponent<Renderer>().material;
        Color ncol = mat.color;
        ncol.a = 1;
        mat.color = ncol;
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
        var tile = gameObject.GetComponent<TileHandler>();
        if (tile == null || tile.IsPositionValid(new Vector2(nx, ny))) {
            transform.position = new Vector3(nx, ny, transform.position.z);
        }
    }
}
