using Unity.VisualScripting;
using UnityEngine;

public class PickUpSoundDestroy : MonoBehaviour
{
    private AudioSource pickuupSound;
    [SerializeField] private float minImpactForce = 3f;

    [SerializeField] private ParticleSystem destroyEffect;
    void Start()
    {
        pickuupSound = GetComponentInChildren<AudioSource>();
    }

    public void DestroyCube()
    {
        if (destroyEffect != null)
        {
            ParticleSystem fx = Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(fx.gameObject, fx.main.duration);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody != null)
            return;

        float impact = collision.relativeVelocity.magnitude;

        if (impact > minImpactForce)
            pickuupSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Danger"))
            return;
        DestroyCube();
    }
}
