using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 10f;
    [SerializeField] float rotationStrength = 7f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rightEngineParticles;
    [SerializeField] ParticleSystem leftEngineParticles;
    Rigidbody rb;
    AudioSource audioSource;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable() 
    {
        thrust.Enable();
        rotation.Enable();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(thrust.IsPressed()){
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime * 100, ForceMode.Force);
            if(!audioSource.isPlaying){
                audioSource.PlayOneShot(mainEngineSound);
            }
            if(!mainEngineParticles.isPlaying){
                mainEngineParticles.Play();
            }
        }else{
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        rb.freezeRotation = true;
        if(rotationInput < 0){
            transform.Rotate(Vector3.forward * rotationStrength * Time.fixedDeltaTime * 10);
            if(!rightEngineParticles.isPlaying){
                leftEngineParticles.Play();
                rightEngineParticles.Play();
            }
        }else if(rotationInput > 0){
            transform.Rotate(Vector3.back * rotationStrength * Time.fixedDeltaTime * 10);
            if(!leftEngineParticles.isPlaying){
                rightEngineParticles.Stop();
                leftEngineParticles.Play();
            }
        }else{
            rightEngineParticles.Stop();
            leftEngineParticles.Stop();
        }
        rb.freezeRotation = false;
    }
}
