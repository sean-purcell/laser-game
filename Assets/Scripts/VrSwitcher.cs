using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VrSwitcher : MonoBehaviour
{
    [Tooltip("Whether to enable or disable VR")]
    public bool vr = true;

    public bool disableVr = false;
    public bool enableVr = false;

    // Start is called before the first frame update
    void Start()
    {
        if (enableVr) {
            EnableVr();
        } else if (disableVr) {
            DisableVr();
        }
    }

    public void EnableVr() {
        StartCoroutine(EnableVrCoro());
    }

    private static IEnumerator EnableVrCoro() {
        string device = "cardboard";

        Debug.Log("Current device: " + XRSettings.loadedDeviceName);

        if (string.Compare(XRSettings.loadedDeviceName, device, true) != 0) {
            Debug.Log("Loading VR");
            XRSettings.LoadDeviceByName(device);
            yield return null;
        }

        if (string.Compare(XRSettings.loadedDeviceName, device, true) != 0) {
            Debug.Log("Failed to load VR");
        } else {
            XRSettings.enabled = true;
            Debug.Log("Loaded device: " + XRSettings.loadedDeviceName);
        }
    }

    public void DisableVr() {
        StartCoroutine(DisableVrCoro());
    }

    private static IEnumerator DisableVrCoro() {
        if (string.Compare(XRSettings.loadedDeviceName, "", true) != 0) {
            XRSettings.LoadDeviceByName("");
            yield return null;
        }
    }
}
