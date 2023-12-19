using MoreMountains.Feedbacks;

using UnityEngine;

public class PlayerEffectHandler : MonoBehaviour
{
    [SerializeField] private MMF_Player hitfeetback;


    public MMF_Player HitFeetback { get => hitfeetback; }



    [SerializeField] private MMF_Player eatfeetback;


    public MMF_Player Eatfeetback { get => eatfeetback; }

    public static PlayerEffectHandler Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }


    public static void PlayHitEffect()
    {
        Instance.HitFeetback?.PlayFeedbacks();
    }

    public static void PlayEatEffect()
    {
        Instance.Eatfeetback?.PlayFeedbacks();
    }

}
