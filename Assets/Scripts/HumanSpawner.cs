using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : ObjectSpawner
{
    public override int GetLimiter()
    {
        return limiter + GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.MaxHumans];
    }

}