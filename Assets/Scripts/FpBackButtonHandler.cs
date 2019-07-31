using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FpBackButtonHandler : MonoBehaviour, IDragHandler
{
    public string mainMenuScene;

    public bool clicked = false;
    public float fill = 0;

    private new Renderer renderer;
    private int fillProperty;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        fillProperty = Shader.PropertyToID("_Fill");
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked) {
            fill += Time.deltaTime;
            if (fill > 1) {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
        renderer.material.SetFloat(fillProperty, fill);
    }

    public void Click()
    {
        clicked = true;
    }

    public void Unclick()
    {
        clicked = false;
        fill = 0;
    }

    public void OnDrag(PointerEventData data)
    {
        // Only implementing this interface to get marked as interactive by GVR
    }
}
