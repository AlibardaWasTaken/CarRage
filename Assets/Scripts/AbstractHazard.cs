using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractHazard : MonoBehaviour
{

    public bool IsRandom;

    [Range(0f,1f)]
    public float SpawnChance;

    public GameObject trapobject;
    
    


    public bool CanRespawn;
    public bool IsRespawWithChance;
    public float RespawnTime;


    private WaitForSeconds WaitCor;
    private void Start()
    {
       


        if (CanRespawn)
        {
            WaitCor = new WaitForSeconds(RespawnTime);
            StartCoroutine(Respawn());
        }



      



        if (IsRandom)
        {
            if (Random.Range(0f, 1f) > SpawnChance)
            {
                trapobject.SetActive(false);
                return;
            }
        }





        if(trapobject.activeInHierarchy)
            DoActionOnSpawn();




    }


   private IEnumerator Respawn()
    {
       
        yield return new WaitForEndOfFrame();
        while (CanRespawn)
        {
            Debug.Log("Can Respawn, starting enum");
            yield return WaitCor;

            if (trapobject.activeInHierarchy == true)
                continue;

            if (IsRespawWithChance)
            {
                if (Random.Range(0f, 1f) > SpawnChance)
                {
                    continue;
                }
            }

            EnableObject();

        }


    }

    public void EnableObject()
    {
        trapobject.SetActive(true);
        DoActionOnSpawn();
    }

    protected virtual void DoActionOnSpawn() 
    { 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trapobject.activeInHierarchy == true && collision.gameObject.TryGetComponent(out TopDownCarController hit))
        {
            if (hit.GetType() == typeof(TopDownEnemyCar))
            {
                DoEnemyAction(hit as TopDownEnemyCar);
                return;
            }
               


            if(hit.IsJumping == false)
            {
                DoAction();
            }
        }
    }

    protected abstract void DoAction();

    protected abstract void DoEnemyAction(TopDownEnemyCar Enemy);
}
