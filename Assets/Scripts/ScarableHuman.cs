using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScarableHuman : BaseHuman
{
    protected bool scared = false;
    [SerializeField]
    private float scareRadius;
    public float ScareRadius { get => scareRadius; set => scareRadius = value; }
    protected override void OnInteract()
    {
        MakeOthersScared();
    }


    protected override void OnHitAction()
    {
        MakeOthersScared();
    }



    private void MakeOthersScared()
    {


        var collidersInRange = Physics2D.OverlapCircleAll(gameObject.transform.position, ScareRadius);

        foreach (var collider in collidersInRange)
        {
            var colliderObject = collider.gameObject;
            var resultComponent = colliderObject.GetComponent<ScarableHuman>();
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
            onScared();
        }
    }

    protected abstract void onScared();
}
