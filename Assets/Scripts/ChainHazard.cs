using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHazard : AbstractHazard
{
    private bool OnCoolDown = false;
    public float CoolDownValue = 10;
    public DistanceJoint2D DistanceJoint;

    public ChainHazardLineUpdater LineUpdater;


    public float CatchTime = 5;
    protected override void DoAction()
    {
        if (OnCoolDown == true)
            return;

        LineUpdater.TargetObject = CarInputHandler.Instance.topDownCarController.gameObject;
        OnCoolDown = true;
        LineUpdater.enabled = true;

    }

    protected override void DoEnemyAction(TopDownEnemyCar Enemy)
    {
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (DistanceJoint.enabled == false && collision.gameObject.TryGetComponent(out TopDownPlayerCar hit))
        {
            if (trapobject.activeInHierarchy == true)
            {
                LineUpdater.enabled = false;
				LineUpdater.IsFixedOn = false;
                StartCoroutine(CoolDownHandler());

            }
        }

    }



    private IEnumerator CoolDownHandler()
    {
        yield return new WaitForSeconds(CoolDownValue);
        OnCoolDown = false;
    }


    public void CreateJoit()
    {
        DistanceJoint.enabled = true;
        DistanceJoint.connectedBody = CarInputHandler.Instance.topDownCarController.CarRigidbody2D;
        StartCoroutine(JointBreakerOnTime());


    }

    private IEnumerator JointBreakerOnTime()
    {
        yield return new WaitForSeconds(CatchTime);

        DestroyJoit();
        LineUpdater.enabled = false;
        StartCoroutine(CoolDownHandler());
    }

    public void DestroyJoit()
    {

        DistanceJoint.enabled = false;
    }
}
