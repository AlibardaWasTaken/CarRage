using System;
using System.Collections;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance = null;



    private int _bloodAmount;
    [SerializeField]
    private int _maxbloodAmount = 100;
    private bool _draining;
    [SerializeField]
    private float _drainSpeed = 0.5f;
    [SerializeField]
    private int _bloodDrainPortion = 1;
    private WaitForSeconds WaitYield;

    public float DrainSpeed { get => _drainSpeed; private set => _drainSpeed = value; }
    public int BloodDrainPortion { get => _bloodDrainPortion; private set => _bloodDrainPortion = value; }
    public int BloodAmount { get => _bloodAmount; private set => _bloodAmount = value; }
    public int MaxbloodAmount { get => _maxbloodAmount + GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Fuel]; set => _maxbloodAmount = value + GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Fuel]; }
    public bool Draining { get => _draining; private set => _draining = value; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }   
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }


   

    }


    private void Start()
    {
        BloodAmount = MaxbloodAmount;
        WaitYield= new WaitForSeconds(DrainSpeed);
        Debug.Log(GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Fuel]);
    }


    public void AddBlood(int amount)
    {
        BloodAmount += amount;
        BloodAmount = Math.Clamp(BloodAmount, 1, MaxbloodAmount);
        HealthBar.Instance.SetBlood(BloodAmount);
    }




    public void RemoveBlood(int amount)
    {
        BloodAmount -= amount;
        BloodAmount = Math.Clamp(BloodAmount, 0, MaxbloodAmount);

        HealthBar.Instance.SetBlood(BloodAmount);
        if (BloodAmount <= 0)
        {
            GameManager.instance.OnRaceCompleted();
        }
    }

    public void StartDrain()
    {
        Draining = true;
        Debug.Log("Max " + MaxbloodAmount);
        StartCoroutine(Drain());
    }

    private IEnumerator Drain()
    {
        while (BloodAmount > 0 && Draining == true)
        {
            RemoveBlood(BloodDrainPortion);

            HealthBar.Instance.SetBlood(BloodAmount);
            yield return WaitYield;
        }

        GameManager.instance.OnRaceCompleted();
        

    }

    public void StopDrain()
    {
        Draining = false;

    }


}
