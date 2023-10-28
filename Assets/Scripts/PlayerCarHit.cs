using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerCarHit : CarHitHandler
{


    [SerializeField] private MMF_Player hitfeetback;



    public MMF_Player HitFeetback { get => hitfeetback; }
    public override void OnHit(Collision2D collision)
    {
        base.OnHit(collision);
        hitfeetback?.PlayFeedbacks();

    }
}
