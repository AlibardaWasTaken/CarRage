using System.Collections;
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
        MakeOthersScared();
        Destroy(this.gameObject);
    }

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;

    private Vector3 _targetDirection;

    [SerializeField]
    private float scareRadius;

    private void OnDestroy()
    {
        onDeath?.Invoke();
    }




    private float _changeDirectionCooldown;

    private WaypointNode currentWaypoint = null;

    public System.Action onDeath;

    private bool scared = false;

    public float ScareRadius { get => scareRadius; set => scareRadius = value; }

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
        _targetDirection = currentWaypoint.transform.position;
    }

    private void UpdateTargetDirection()
    {
        //HandleRandomDirectionChange();
        if (scared == false && currentWaypoint != null && Vector2.Distance(this.transform.position, currentWaypoint.transform.position) < currentWaypoint.nextWaypointNode.minDistanceToReachWaypoint)
        {
           
            currentWaypoint = currentWaypoint.nextWaypointNode;
            _targetDirection = currentWaypoint.transform.position;
        }

    }


    private void MakeOthersScared()
    {

        
        var collidersInRange = Physics2D.OverlapCircleAll(gameObject.transform.position, ScareRadius);
        Debug.Log(collidersInRange.Length);
        foreach (var collider in collidersInRange)
        {
            var colliderObject = collider.gameObject;
            var resultComponent = colliderObject.GetComponent<HumanNpc>();
            if (resultComponent != null)
            {
        
                resultComponent.MakeThisScared();
            }
        }
    }
    public void MakeThisScared()
    {
        if (scared == false)
        {
           
            scared = true;
            StartCoroutine(ScaredWalk());
        }
    }
   private IEnumerator ScaredWalk()
    {

           
            _changeDirectionCooldown = Random.Range(2f, 5f);
            _changeDirectionCooldown -= Time.deltaTime;
            while (_changeDirectionCooldown > 0)
            {
                yield return new WaitForEndOfFrame();
                float angleChange = Random.Range(-90f, 90f);
                Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                _targetDirection = rotation * _targetDirection;
            }
        FindClosestWayPoint();
            scared = false;
        
        
    }




    private void RotateTowardsTarget()
    {
        float angle = (Mathf.Atan2(_targetDirection.y - transform.position.y, _targetDirection.x - transform.position.x) * Mathf.Rad2Deg) - 90;
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
        MakeOthersScared();
        Destroy(this.gameObject);
    }
}
