using System;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car settings")]
    [SerializeField] private float driftFactor = 0.95f;
    [SerializeField] private float accelerationFactor = 30.0f;
    [SerializeField] private float turnFactor = 3.5f;
    [SerializeField] private float maxSpeed = 20;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer carSpriteRenderer;
    [SerializeField] private SpriteRenderer carShadowRenderer;

    [Header("Jumping")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private ParticleSystem landingParticleSystem;


    //Local variables
    [SerializeField] private float accelerationInput = 0;
    [SerializeField] private float steeringInput = 0;

    [SerializeField] private float rotationAngle = 0;

    [SerializeField] private float velocityVsUp = 0;

    private bool isjumping = false;

    //Components
    private Rigidbody2D carRigidbody2D;
    private Collider2D carCollider;
    private CarSFXHandler carSfxHandler;

    #region enc
    public float DriftFactor { get => driftFactor; set => driftFactor = value; }
    public float AccelerationFactor { get => accelerationFactor; set => accelerationFactor = value; }
    public float TurnFactor { get => turnFactor; set => turnFactor = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public SpriteRenderer CarSpriteRenderer { get => carSpriteRenderer;  }
    public SpriteRenderer CarShadowRenderer { get => carShadowRenderer; }
    public AnimationCurve JumpCurve { get => jumpCurve; }
    public ParticleSystem LandingParticleSystem { get => landingParticleSystem;  }
    public float AccelerationInput { get => accelerationInput; set => accelerationInput = value; }
    public float SteeringInput { get => steeringInput; set => steeringInput = value; }
    public float RotationAngle { get => rotationAngle; set => rotationAngle = value; }
    public float VelocityVsUp { get => velocityVsUp; set => velocityVsUp = value; }
    public bool IsJumping { get => isjumping; set => isjumping = value; }
    public Rigidbody2D CarRigidbody2D { get => carRigidbody2D; protected set => carRigidbody2D = value; }
    public Collider2D CarCollider { get => carCollider; protected set => carCollider = value; }
    public CarSFXHandler CarSfxHandler { get => carSfxHandler; protected set => carSfxHandler = value; }







    #endregion

    void Awake()
    {
        CarRigidbody2D = GetComponent<Rigidbody2D>();
        CarCollider = GetComponent<Collider2D>();
        CarSfxHandler = GetComponent<CarSFXHandler>();
    }

    void Start()
    {
        RotationAngle = transform.rotation.eulerAngles.z;
        OnStart();
    }


    internal virtual void OnStart() { }

    void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() == GameStates.countDown)
            return;

        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();

        OnFixedUpdate();
    }

    internal virtual void OnFixedUpdate() { }

    void ApplyEngineForce()
    {

        if (IsJumping && AccelerationInput < 0)
            AccelerationInput = 0;

   
        if (AccelerationInput == 0)
            CarRigidbody2D.drag = Mathf.Lerp(CarRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else CarRigidbody2D.drag = 0;

   
        VelocityVsUp = Vector2.Dot(transform.up, CarRigidbody2D.velocity);

  
        if (VelocityVsUp > MaxSpeed && AccelerationInput > 0)
            return;


        if (VelocityVsUp < -MaxSpeed * 0.5f && AccelerationInput < 0)
            return;

     
        if (CarRigidbody2D.velocity.sqrMagnitude > MaxSpeed * MaxSpeed && AccelerationInput > 0 && !IsJumping)
            return;

  
        Vector2 engineForceVector = transform.up * AccelerationInput * AccelerationFactor;

        
        CarRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
     
        float minSpeedBeforeAllowTurningFactor = (CarRigidbody2D.velocity.magnitude / 2);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);


        RotationAngle -= SteeringInput * TurnFactor * minSpeedBeforeAllowTurningFactor;


        CarRigidbody2D.MoveRotation(RotationAngle);
    }

    void KillOrthogonalVelocity()
    {

        Vector2 forwardVelocity = transform.up * Vector2.Dot(CarRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(CarRigidbody2D.velocity, transform.right);


        CarRigidbody2D.velocity = forwardVelocity + rightVelocity * DriftFactor;
    }

    float GetLateralVelocity()
    {

        return Vector2.Dot(transform.right, CarRigidbody2D.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (IsJumping)
            return false;

  
        if (AccelerationInput < 0 && VelocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
            return true;

        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        SteeringInput = inputVector.x;
        AccelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return CarRigidbody2D.velocity.magnitude;
    }






}