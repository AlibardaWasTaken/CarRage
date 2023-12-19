using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class HumanNpc : ScarableHuman
{



    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;


    private Vector3 _targetDirection;
   

    private WaypointNode currentWaypoint = null;





    protected override void OnStart()
    {

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
        if (scared == true) return;

        //HandleRandomDirectionChange();
        if (currentWaypoint != null && Vector2.Distance(this.transform.position, currentWaypoint.transform.position) < currentWaypoint.nextWaypointNode.minDistanceToReachWaypoint)
        {
           
            currentWaypoint = currentWaypoint.nextWaypointNode;
            _targetDirection = currentWaypoint.transform.position;
        }

    }


    protected override void onScared()
    {
        StartCoroutine(ScaredWalk());
 
    }

    WaitForEndOfFrame FrameWait = new WaitForEndOfFrame();
    private IEnumerator ScaredWalk()
    {

           
           var changeDirectionCooldown = Random.Range(2f, 5f);
            

            while (changeDirectionCooldown > 0)
            {
            yield return FrameWait;
            float angleChange = Random.Range(-90f, 90f);
                Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                _targetDirection = rotation * _targetDirection;
            changeDirectionCooldown -= Time.deltaTime;
            }
        scared = false;
        FindClosestWayPoint();
        
        
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


}
