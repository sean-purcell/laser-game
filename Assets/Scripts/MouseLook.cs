using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Vector2 sensitivity = new Vector2(1, 1);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity.x;
        float rotationY = -transform.localEulerAngles.x;
        if (rotationY < -180) {
            rotationY += 360;
        }
        rotationY += Input.GetAxis("Mouse Y") * sensitivity.y;

        rotationX = Mathf.Repeat(rotationX, 360);
        rotationY = Mathf.Clamp(rotationY, -85, 85);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
