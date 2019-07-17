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
            enableVr();
        } else {
            disableVr();
        }
    }

    public void enableVr() {
        StartCoroutine(enableVrCoro());
    }

    private static IEnumerator enableVrCoro() {
        string device = "cardboard";

        if (string.Compare(XRSettings.loadedDeviceName, device, true) != 0) {
            XRSettings.LoadDeviceByName(device);
            yield return null;
        }

        XRSettings.enabled = true;
    }

    public void disableVr() {
        StartCoroutine(disableVrCoro());
    }

    private static IEnumerator disableVrCoro() {
        XRSettings.LoadDeviceByName("");
        yield return null;
    }
}
