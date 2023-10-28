using UnityEngine;
using UnityEngine.Audio;

public class CarSFXHandler : MonoBehaviour
{
    [Header("Mixers")]
    private AudioMixer audioMixer;

    [Header("Audio sources")]
    [SerializeField] private AudioSource tiresScreeachingAudioSource;
    [SerializeField] private AudioSource engineAudioSource;
    [SerializeField] private AudioSource carHitAudioSource;
    [SerializeField] private AudioSource carJumpAudioSource;
    [SerializeField] private AudioSource carJumpLandingAudioSource;


    //Local variables
    [SerializeField] private float desiredEnginePitch = 0.5f;
    [SerializeField] private float tireScreechPitch = 0.5f;

    //Components
    private TopDownCarController topDownCarController;

    public AudioMixer AudioMixer { get => audioMixer; set => audioMixer = value; }
    public AudioSource TiresScreeachingAudioSource { get => tiresScreeachingAudioSource; set => tiresScreeachingAudioSource = value; }
    public AudioSource EngineAudioSource { get => engineAudioSource; set => engineAudioSource = value; }
    public AudioSource CarHitAudioSource { get => carHitAudioSource; set => carHitAudioSource = value; }
    public AudioSource CarJumpAudioSource { get => carJumpAudioSource; set => carJumpAudioSource = value; }
    public AudioSource CarJumpLandingAudioSource { get => carJumpLandingAudioSource; set => carJumpLandingAudioSource = value; }
    public float DesiredEnginePitch { get => desiredEnginePitch; set => desiredEnginePitch = value; }
    public float TireScreechPitch { get => tireScreechPitch; set => tireScreechPitch = value; }
    public TopDownCarController TopDownCarController { get => topDownCarController; }


    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Example for recording, move this part to any setting script that your game might use. 
        //audioMixer.SetFloat("SFXVolume", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        //Handle engine SFX
        float velocityMagnitude = TopDownCarController.GetVelocityMagnitude();

        //Increase the engine volume as the car goes faster
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        //But keep a minimum level so it playes even if the car is idle
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);

        EngineAudioSource.volume = Mathf.Lerp(EngineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        //To add more variation to the engine sound we also change the pitch
        DesiredEnginePitch = velocityMagnitude * 0.2f;
        DesiredEnginePitch = Mathf.Clamp(DesiredEnginePitch, 0.5f, 2f);
        EngineAudioSource.pitch = Mathf.Lerp(EngineAudioSource.pitch, DesiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateTiresScreechingSFX()
    {
        //Handle tire screeching SFX
        if (TopDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            //If the car is braking we want the tire screech to be louder and also change the pitch.
            if (isBraking)
            {
                TiresScreeachingAudioSource.volume = Mathf.Lerp(TiresScreeachingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                TireScreechPitch = Mathf.Lerp(TireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                //If we are not braking we still want to play this screech sound if the player is drifting.
                TiresScreeachingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                TireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        //Fade out the tire screech SFX if we are not screeching. 
        else TiresScreeachingAudioSource.volume = Mathf.Lerp(TiresScreeachingAudioSource.volume, 0, Time.deltaTime * 10);
    }

    public void PlayJumpSfx()
    {
        CarJumpAudioSource.Play();
    }

    public void PlayLandingSfx()
    {
        CarJumpLandingAudioSource.Play();
    }

    public void PlayHitSound(float relativeVelocity)
    {
        //Get the relative velocity of the collision

        float volume = relativeVelocity * 0.1f;

        CarHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        CarHitAudioSource.volume = volume;

        if (!CarHitAudioSource.isPlaying)
            CarHitAudioSource.Play();
    }


}
