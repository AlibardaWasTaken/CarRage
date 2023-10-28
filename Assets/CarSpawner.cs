using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : ObjectSpawner
{


    [SerializeField]
    private int CarLimiter = 100;

    private int CarAmount = 0;

    protected override bool Condition()
    {
        return (CarLimiter > CarAmount);
    }
    protected override void OnSpawn(GameObject SpawnedObj)
    {
        CarAmount++;
        SpawnedObj.GetComponent<TopDownEnemyCar>().onDeath += RemoveCarAmount;
    }


    public void RemoveCarAmount()
    {
        CarAmount--;
    }
}
