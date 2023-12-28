using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TopDownEnemyCar : TopDownCarController, Iinteractable, IPoolLinked
{

    ObjectPool<GameObject> IPoolLinked.LinkedPool { get; set; }


    public GameObject ThisGameObject()
    {
        return this.gameObject;
    }



    [SerializeField]
    private int _health = 2;
    
    private bool AnimatingHit = false;





    public int Health { get => _health; private set => _health = value; }

    public void SetInitialHp(int Hp)
    {
        var floorTime = (int)Mathf.Floor(GameManager.instance.GetRaceTime() / 60); // 1 хп машины раз в 1 минуту
        floorTime = Math.Clamp(floorTime, 0, 20);
        Health = Hp + floorTime;
        
    }

    public void Interact()
    {
        DoDamage(1 + GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Crusher]);
    }








    private IEnumerator CarHitted()
    {
        CarSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        CarSpriteRenderer.color = Color.white;
        AnimatingHit = false;
    }

    public override void DoDamage(int amount)
    {
        Health -= amount;
        if (AnimatingHit == false)
        {
            AnimatingHit = true;
            StartCoroutine(CarHitted());
        }

        if (Health <= 0)
        {
            Instantiate(ParticleHolder.ExpParticle, this.transform.position, Quaternion.identity, null);
            CommonSoundManager.Instance.ExpContainer.PlayRandom();
            Destroy(this.gameObject);
        }
    }
}
