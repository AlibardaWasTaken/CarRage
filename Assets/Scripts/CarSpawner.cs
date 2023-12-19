using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : ObjectSpawner
{
    protected override void OnPoolGet(GameObject obj)
    {
        base.OnPoolGet(obj);
        var CarComp = obj.GetComponent<TopDownEnemyCar>();


        CarComp.SetInitialHp(Random.Range(2, 6));
    }

}

