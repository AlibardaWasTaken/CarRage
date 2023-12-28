using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatistHumanNpc : ScarableHuman
{

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;


    private Vector3 _targetDirection;


    protected override void onScared()
    {
        StartCoroutine(ScaredWalk());
  
    }

    WaitForEndOfFrame FrameWait = new WaitForEndOfFrame();

    private void RotateTowardsTarget()
    {
        float angle = (Mathf.Atan2(_targetDirection.y - transform.position.y, _targetDirection.x - transform.position.x) * Mathf.Rad2Deg) - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }


    private IEnumerator ScaredWalk()
    {


        var changeDirectionCooldown = Random.Range(2f, 5f);


        while (changeDirectionCooldown > 0)
        {
            yield return FrameWait;
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;


            RotateTowardsTarget();
            _rigidbody.velocity = transform.up * _speed;



            changeDirectionCooldown -= Time.deltaTime;
        }
        scared = false;



    }

}
