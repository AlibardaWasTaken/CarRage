using System;
using System.Collections;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance = null;



    private int bloodAmount;
    private int MaxbloodAmount = 100;
    [SerializeField] private bool draining;
    public int BloodAmount { get => bloodAmount; }
    public bool Draining { get => draining; set => draining = value; }

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


     
        bloodAmount = 100;
        

    }


    public void AddBlood(int amount)
    {
        bloodAmount += amount;
        bloodAmount = Math.Clamp(bloodAmount, 1, MaxbloodAmount);
        HealthBar.Instance.SetBlood(bloodAmount);
    }




    public void RemoveBlood(int amount)
    {
        bloodAmount -= amount;
        bloodAmount = Math.Clamp(bloodAmount, 0, MaxbloodAmount);

        HealthBar.Instance.SetBlood(bloodAmount);
        if (bloodAmount <= 0)
        {
            GameManager.instance.OnRaceCompleted();
        }
    }

    public void StartDrain()
    {
        draining = true;
        StartCoroutine(Drain());
    }

    private IEnumerator Drain()
    {
        while (bloodAmount > 0 && draining == true)
        {
            bloodAmount--;
            HealthBar.Instance.SetBlood(bloodAmount);
            yield return new WaitForSeconds(0.5f);
        }
        if (draining == true)
        {
            GameManager.instance.OnRaceCompleted();
        }

    }

    public void StopDrain()
    {
        draining = false;

    }


}
