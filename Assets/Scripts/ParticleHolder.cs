using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHolder : MonoBehaviour
{

    public static ParticleHolder Instance;


    [SerializeField]
    private ParticleSystem expParticle;

    [SerializeField]
    private ParticleSystem bloodExp;

    [SerializeField]
    private ParticleSystem woodExp;

    [SerializeField]
    private ParticleSystem collideParticle;

    public static ParticleSystem ExpParticle { get => Instance.expParticle; }
    public static ParticleSystem BloodExp { get => Instance.bloodExp;  }
    public static ParticleSystem WoodExp { get => Instance.woodExp; }
    public static ParticleSystem CollideParticle { get => Instance.collideParticle;  }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }


        Destroy(this);
    }





}
