using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : TileHandler
{
    // Time the target must be active for before marking itself active
    public float cutoff = 0.5f;

    private int layerMask;

    private float activeTime;

    private Renderer renderer;
    private int fillProperty;
    private int fillColourProperty;
    private int emissionProperty;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Beam");

        activeTime = 0;

        renderer = GetComponent<Renderer>();
        fillProperty = Shader.PropertyToID("_Fill");
        fillColourProperty = Shader.PropertyToID("_FillColour");
        emissionProperty = Shader.PropertyToID("_Emission");
    }

    void Update()
    {
        float fill;
        if (cutoff <= 1e-4) {
            fill = IsActive() ? 1 : 0;
        } else {
            fill = Mathf.Min(1.0f, activeTime / cutoff);
        }
        renderer.material.SetFloat(fillProperty, fill);
    }

    public bool IsActive()
    {
        // Activate instantly, but still require a collision
        if (cutoff <= 1e-4) {
            return activeTime > 0;
        }
        return activeTime >= cutoff;
    }

    public override void Process(float dt)
    {
        // Check for any collisions with lasers

        var collisions = Physics.OverlapSphere(
                transform.position,
                transform.localScale.x / 2,
                layerMask);
        if (collisions.Length > 0) {
            activeTime += dt;
        } else {
            activeTime = 0;
        }
    }

    public void ShowWin()
    {
        // Change fill colour to green & turn on emission
        renderer.material.SetColor(fillColourProperty, Color.green);
        renderer.material.SetFloat(emissionProperty, 1);
    }
}
