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
            EnableVr();
        } else {
            DisableVr();
        }
    }

    public void EnableVr() {
        StartCoroutine(EnableVrCoro());
    }

    private static IEnumerator EnableVrCoro() {
        string device = "cardboard";

        if (string.Compare(XRSettings.loadedDeviceName, device, true) != 0) {
            XRSettings.LoadDeviceByName(device);
            yield return null;
        }

        XRSettings.enabled = true;
    }

    public void DisableVr() {
        StartCoroutine(DisableVrCoro());
    }

    private static IEnumerator DisableVrCoro() {
        XRSettings.LoadDeviceByName("");
        yield return null;
    }
}
