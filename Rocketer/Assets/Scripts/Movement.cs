using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;

    [SerializeField] private float thrustStrenght = 100f;
    [SerializeField] private float rotationStrenght = 100f;

    [SerializeField] private AudioClip engine;

    [SerializeField] private ParticleSystem mainEngine;
    [SerializeField] private ParticleSystem sideREngine;
    [SerializeField] private ParticleSystem sideLEngine;

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }
    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            audioSource.Stop();
            mainEngine.Stop();
        }
    }

    private void StartThrusting()
    {
        //reliativeforce, daltego, ¿e chcemy alby loclnie by³ dodwany ten force( czyli jak siê obróci to góra bêdzie 
        //w kierunku co siê odwróci³o
        rb.AddRelativeForce(Vector3.up * thrustStrenght * Time.fixedDeltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engine);
        }

        if (!mainEngine.isPlaying)
        {
            mainEngine.Play();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        

        if(rotationInput < 0)
        {
            ApplyRotation(rotationStrenght);
            sideLEngine.Play(); 
            sideREngine.Stop();
        }
        else if(rotationInput > 0)
        {
            ApplyRotation(-rotationStrenght);
            sideREngine.Play();
            sideLEngine.Stop();
        }
        else
        {
            sideLEngine.Stop();
            sideREngine.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
