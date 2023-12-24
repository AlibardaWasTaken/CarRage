using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : ObjectSpawner
{

    protected override void OnStart()
    {
        StartCoroutine(SpawnLessHumans());
    }
    public override int GetLimiter()
    {

        return limiter + GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.MaxHumans];
    }


    private WaitForSeconds CashedTime = new WaitForSeconds(30);
 private IEnumerator SpawnLessHumans()
    {
        while (SpawnInterval < 1.3)
        {
            yield return CashedTime;
            SpawnInterval += 0.15f;
        }
    }

}