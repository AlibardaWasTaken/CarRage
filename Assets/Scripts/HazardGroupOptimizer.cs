using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardGroupOptimizer : MonoBehaviour
{


    public AbstractHazard ObjectToSync;
    public List<AbstractHazard> HazardList = new List<AbstractHazard>();


    private WaitForSeconds WaitCor;

    private void Start()
    {

        foreach (var hazard in HazardList)
        {
            hazard.trapobject.SetActive(ObjectToSync.trapobject.activeInHierarchy);
            hazard.CanRespawn = false;
        }

        WaitCor = new WaitForSeconds(ObjectToSync.RespawnTime);
        StartCoroutine(RespawnGroup());
    }



    private IEnumerator RespawnGroup()
    {

        yield return new WaitForEndOfFrame();
        while (true)
        {
            
            yield return WaitCor;


            foreach (var haz in HazardList)
            {

                if (haz.trapobject.activeInHierarchy == true)
                    continue;

                if (haz.IsRespawWithChance)
                {
                    if (Random.Range(0f, 1f) > ObjectToSync.SpawnChance)
                    {
                        continue;
                    }
                }

                haz.EnableObject();
            }


        }


    }


}
