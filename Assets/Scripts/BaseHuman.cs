using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseHuman : MonoBehaviour, Iinteractable, IHittable, IPoolLinked
{
    ObjectPool<GameObject> IPoolLinked.LinkedPool { get; set; }

    protected Rigidbody2D _rigidbody;


    public void Interact()
    {
        BloodManager.Instance.AddBlood(Random.Range(4, 7) + GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Crusher]);
        CommonSoundManager.Instance.ScreameffectContainer.PlayRandom();
        CommonSoundManager.Instance.PlayRandomGrindSound();
        CarInputHandler.Instance.topDownCarController.Eat();
        GameManager.AddPoints(1 + GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Points]);
        Instantiate(ParticleHolder.BloodExp, this.transform.position, Quaternion.identity);


        OnInteract();


        // PlayerEffectHandler.PlayEatEffect();
        ((IPoolLinked)this).ReturnPool();
    }





    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        OnStart();
    }


    protected virtual void OnStart()
    {
    }

    protected virtual void OnInteract()
    {
    }



    protected virtual void OnHitAction()
    {
    }

    public void OnHit()
    {
        CommonSoundManager.Instance.ScreameffectContainer.PlayRandom();
        Instantiate(ParticleHolder.BloodExp, this.transform.position, Quaternion.identity);
        OnHitAction();
        ((IPoolLinked)this).ReturnPool();
    }

    public GameObject ThisGameObject()
    {
        return this.gameObject;
    }





}
