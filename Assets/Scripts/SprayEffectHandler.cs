using UnityEngine;
using System.Collections;

public class SprayEffectHandler : MonoBehaviour
{
    public ParticleSystem sprayEffect;

    // Use this for initialization
    void Start()
    {
        sprayEffect = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDestroy()
    {
        GameObject.Destroy(gameObject);
    }

    public void Play(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
        sprayEffect.Play();
    }

    public void Stop()
    {
        sprayEffect.Stop(); // no-op if not playing
    }
}
