using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHazardLineUpdater : MonoBehaviour
{
    public LineRenderer Line;

    private Vector3 movecord;

    [HideInInspector]
    public GameObject TargetObject;

    public float MoveSpeed = 2;

    [HideInInspector]
    public bool IsFixedOn = false;

    public ChainHazard MainHazard;



    private void Update()
    {
        if(IsFixedOn == true)
        {
            Line.SetPosition(1, TargetObject.transform.position);
            return;
        }


        movecord = Vector3.MoveTowards(Line.GetPosition(1), TargetObject.transform.position, MoveSpeed * Time.deltaTime) ;
        Line.SetPosition(1, movecord);
        CheckDistanceToObject();


    }



    private void CheckDistanceToObject()
    {

        IsFixedOn = Vector3.Distance(Line.GetPosition(1), TargetObject.transform.position) <= 2;

        if (IsFixedOn == true)
            MainHazard.CreateJoit();
    }

    private void OnEnable()
    {
        Line.SetPosition(0, transform.position);
        Line.SetPosition(1, transform.position);

    }
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;

        Instantiate(ParticleHolder.ExpParticle, Line.GetPosition(1), Quaternion.identity, null);

        Line.SetPosition(0, transform.position);
        Line.SetPosition(1, transform.position);
    }

}
