using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeaconHandler : MonoBehaviour, IDragHandler
{
    public bool playerHere;

    private bool beaconMode;

    private new Renderer renderer;
    private new Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        beaconMode = GameObject
            .Find("/PuzzleBase/Player")
            .GetComponentInChildren<PlayerHandler>()
            .teleportMode == PlayerHandler.TeleportMode.Beacon;

        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();

        UpdateEnabled();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateEnabled()
    {
        bool enabled = !playerHere && beaconMode;
        renderer.enabled = enabled;
        collider.enabled = enabled;
    }

    public void Enter()
    {
        playerHere = true;
        UpdateEnabled();
    }

    public void Leave()
    {
        playerHere = false;
        UpdateEnabled();
    }

    public void OnDrag(PointerEventData data)
    {
        // Only implementing this interface to get marked as interactive by GVR
    }
}
