using Unity.Cinemachine;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private ParticleSystem PS;
    [SerializeField] private float shakeModifier = 10f;
    private CinemachineImpulseSource cinemachineImpulseSource;

    [SerializeField] private float collisionCooldown = 1f;

    private float collisionTimer = 0f;

    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        collisionTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionTimer < collisionCooldown) return;

        FireImpuls();
        CollisonFX(collision);
        collisionTimer = 0f;
    }

    private void FireImpuls()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float shakeIntensity = (1f / distance) * shakeModifier;
        shakeIntensity = Mathf.Min(shakeIntensity, 1f);

        cinemachineImpulseSource.GenerateImpulse(shakeIntensity);
    }
    private void CollisonFX(Collision other)
    {
        ContactPoint contactPoint = other.contacts[0];
        PS.transform.position = contactPoint.point;
        PS.Play();
    }
}
