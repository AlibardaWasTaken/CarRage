using System.Collections;
using UnityEngine;

public class CommonSoundManager : MonoBehaviour
{
    public static CommonSoundManager Instance;
    [SerializeField] private SoundEffectContainer screameffectContainer;
    [SerializeField] private SoundEffectContainer GrindffectContainer;
    [SerializeField] private SoundEffectContainer musiceffectContainer;
    [SerializeField] private SoundEffectContainer expContainer;
    [SerializeField] private GameObject soundObject;
    private bool Grinding;

    public SoundEffectContainer ScreameffectContainer { get => screameffectContainer; }
    public SoundEffectContainer MusiceffectContainer { get => musiceffectContainer; }
    public GameObject SoundObject { get => soundObject; }
    public SoundEffectContainer ExpContainer { get => expContainer; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);


    }




    public void PlayRandomGrindSound()
    {
        if (Grinding == false)
        {
            StartCoroutine(PlayGrindSound());
        }


    }

    private IEnumerator PlayGrindSound()
    {
        Grinding = true;
        var clip = GrindffectContainer.PlayRandom();

        yield return new WaitForSeconds(clip.length);
        Grinding = false;


    }

    public void ChangeMusic()
    {

    }

}





