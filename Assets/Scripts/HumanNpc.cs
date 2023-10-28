using System.Linq;
using UnityEngine;

public class HumanNpc : MonoBehaviour, Iinteractable, IHittable
{
    public void Interact()
    {
        BloodManager.Instance.AddBlood(Random.Range(4, 10));
        CommonSoundManager.Instance.ScreameffectContainer.PlayRandom();
        CommonSoundManager.Instance.PlayRandomGrindSound();
        CarInputHandler.Instance.topDownCarController.Eat();
        Instantiate((GameObject)Resources.Load("BloodExp"), this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;

    private Vector2 _targetDirection;


    private void OnDestroy()
    {
        onDeath?.Invoke();
    }




    private float _changeDirectionCooldown;

    private WaypointNode currentWaypoint = null;

    public System.Action onDeath;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _targetDirection = transform.up;

        FindClosestWayPoint();
    }

    private void FixedUpdate()
    {
        SetVelocity();
        UpdateTargetDirection();
        RotateTowardsTarget();
        
    }


   private void FindClosestWayPoint()
    {
        currentWaypoint = WayPointCalculator.FindClosestWayPoint(this.gameObject.transform);
    }

    private void UpdateTargetDirection()
    {
        //HandleRandomDirectionChange();
        if (currentWaypoint != null && Vector2.Distance(this.transform.position, currentWaypoint.transform.position) < currentWaypoint.nextWaypointNode.minDistanceToReachWaypoint)
        {
           
            currentWaypoint = currentWaypoint.nextWaypointNode;
        }

    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(4f, 15f);
        }
    }



    private void RotateTowardsTarget()
    {
        float angle = (Mathf.Atan2(currentWaypoint.transform.position.y - transform.position.y, currentWaypoint.transform.position.x - transform.position.x) * Mathf.Rad2Deg) - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void SetVelocity()
    {
        _rigidbody.velocity = transform.up * _speed;
    }

    public void OnHit()
    {
        CommonSoundManager.Instance.ScreameffectContainer.PlayRandom();
        Instantiate((GameObject)Resources.Load("BloodExp"), this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
