using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandler : MonoBehaviour
{
    public Image playerMovementImage;

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
    }

    public void HidePlayerMovement()
    {
        playerMovementImage.enabled = false;
    }
}
