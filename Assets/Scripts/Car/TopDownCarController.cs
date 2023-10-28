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
    [SerializeField] private Rigidbody2D carRigidbody2D;
    [SerializeField] private Collider2D carCollider;
    [SerializeField] private CarSFXHandler carSfxHandler;

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


    public Rigidbody2D CarRigidbody2D { get => carRigidbody2D; }
    public Collider2D CarCollider { get => carCollider; }
    public CarSFXHandler CarSfxHandler { get => carSfxHandler; }

    #endregion
    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponentInChildren<Collider2D>();
        carSfxHandler = GetComponent<CarSFXHandler>();
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
        //Don't let the player brake while in the air, but we still allow some drag so it can be slowed slightly. 
        if (IsJumping && AccelerationInput < 0)
            AccelerationInput = 0;

        //Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (AccelerationInput == 0)
            CarRigidbody2D.drag = Mathf.Lerp(CarRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else CarRigidbody2D.drag = 0;

        //Caculate how much "forward" we are going in terms of the direction of our velocity
        VelocityVsUp = Vector2.Dot(transform.up, CarRigidbody2D.velocity);

        //Limit so we cannot go faster than the max speed in the "forward" direction
        if (VelocityVsUp > MaxSpeed && AccelerationInput > 0)
            return;

        //Limit so we cannot go faster than the 50% of max speed in the "reverse" direction
        if (VelocityVsUp < -MaxSpeed * 0.5f && AccelerationInput < 0)
            return;

        //Limit so we cannot go faster in any direction while accelerating
        if (CarRigidbody2D.velocity.sqrMagnitude > MaxSpeed * MaxSpeed && AccelerationInput > 0 && !IsJumping)
            return;

        //Create a force for the engine
        Vector2 engineForceVector = transform.up * AccelerationInput * AccelerationFactor;

        //Apply force and pushes the car forward
        CarRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        //Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (CarRigidbody2D.velocity.magnitude / 2);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //Update the rotation angle based on input
        RotationAngle -= SteeringInput * TurnFactor * minSpeedBeforeAllowTurningFactor;

        //Apply steering by rotating the car object
        CarRigidbody2D.MoveRotation(RotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        //Get forward and right velocity of the car
        Vector2 forwardVelocity = transform.up * Vector2.Dot(CarRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(CarRigidbody2D.velocity, transform.right);

        //Kill the orthogonal velocity (side velocity) based on how much the car should drift. 
        CarRigidbody2D.velocity = forwardVelocity + rightVelocity * DriftFactor;
    }

    float GetLateralVelocity()
    {
        //Returns how how fast the car is moving sideways. 
        return Vector2.Dot(transform.right, CarRigidbody2D.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (IsJumping)
            return false;

        //Check if we are moving forward and if the player is hitting the brakes. In that case the tires should screech.
        if (AccelerationInput < 0 && VelocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        //If we have a lot of side movement then the tires should be screeching
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