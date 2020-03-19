using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandler : MonoBehaviour
{
    public Image playerMovementImage;

    public MeshRenderer gvrReticleRenderer;

    // Start is called before the first frame update
    void Start()
    {
        this.HidePlayerMovement();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void ShowPlayerMovement()
    {
        playerMovementImage.enabled = true;
        HideReticle();
    }

    public void HidePlayerMovement()
    {
        playerMovementImage.enabled = false;
        ShowReticle();
    }

    public void ShowReticle()
    {
        gvrReticleRenderer.enabled = true;
    }

    public void HideReticle()
    {
        gvrReticleRenderer.enabled = false;
    }
}
