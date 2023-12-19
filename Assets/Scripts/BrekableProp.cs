using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrekableProp : MonoBehaviour, IHittable
{
    public void OnHit()
    {
        Instantiate(ParticleHolder.WoodExp, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
