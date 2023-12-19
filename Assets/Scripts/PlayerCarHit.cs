using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerCarHit : CarHitHandler
{



    public override void OnHit(Collision2D collision)
    {
        base.OnHit(collision);
        PlayerEffectHandler.PlayHitEffect();

    }
}
