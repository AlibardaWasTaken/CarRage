using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEnemyCar : TopDownCarController, Iinteractable
{
    public Action onDeath;
    [SerializeField]
    private int health = 2;
    
    private bool AnimatingHit = false;
  




    public int Health { get => health; set => health = value; }

    public void Interact()
    {
        Health--;
        if (AnimatingHit == false)
        {
            AnimatingHit = true;
            StartCoroutine(CarHitted());
        }

        if(health <= 0)
        {
            Instantiate(Resources.Load("ExpParticle"), this.transform.position,Quaternion.identity,null);
            CommonSoundManager.Instance.ExpContainer.PlayRandom();
            onDeath?.Invoke();
            Destroy(this.gameObject);
        }
    }

    private IEnumerator CarHitted()
    {
        CarSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        CarSpriteRenderer.color = Color.white;
        AnimatingHit = false;
    }
}
