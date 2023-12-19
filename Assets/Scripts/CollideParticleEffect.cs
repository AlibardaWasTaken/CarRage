using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideParticleEffect : MonoBehaviour
{

    public int SpeedToTrigger = 20;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb) )
        {
            if (rb.velocity.sqrMagnitude > SpeedToTrigger)
            {
                Instantiate(ParticleHolder.CollideParticle, collision.contacts[0].point, Quaternion.identity, this.gameObject.transform);
            }
        }

    }

}
