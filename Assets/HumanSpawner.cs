using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : ObjectSpawner
{
    [SerializeField]
    private int humanLimiter = 100;

    private int humanAlive = 0;

    protected override bool Condition()
    {
        return (humanLimiter > humanAlive);
    }


    protected override void OnSpawn(GameObject SpawnedObj)
    {
        humanAlive++;
        SpawnedObj.GetComponent<HumanNpc>().onDeath += RemoveHumanAmount;
    }


    public void RemoveHumanAmount()
    {
        humanAlive--;
    }



}