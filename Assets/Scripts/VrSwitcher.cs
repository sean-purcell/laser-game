using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VrSwitcher : MonoBehaviour
{
    [Tooltip("Whether to enable or disable VR")]
    public bool vr = true;

    // Start is called before the first frame update
    void Start()
    {
        if (vr) {
            StartCoroutine(enableVr());
        } else {
            StartCoroutine(disableVr());
        }
    }

    public static IEnumerator enableVr() {
        string device = "cardboard";

        if (string.Compare(XRSettings.loadedDeviceName, device, true) != 0) {
            XRSettings.LoadDeviceByName(device);
            yield return null;
        }

        XRSettings.enabled = true;
    }

    public static IEnumerator disableVr() {
        XRSettings.LoadDeviceByName("");
        yield return null;
    }
}
