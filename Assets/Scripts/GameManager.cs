using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver };

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    //Static instance of GameManager so other scripts can access it
    public static GameManager instance = null;

    //States
   private GameStates gameState = GameStates.countDown;

    //Time
   private float raceStartedTime = 0;
   private float raceCompletedTime = 0;



    [SerializeField]
    private List<ShopItemScritpableObject> _shopItems = new List<ShopItemScritpableObject>();
    public List<ShopItemScritpableObject> ShopItems { get => _shopItems; set => _shopItems = value; }
  

    private static UpgradeValueHolder _valueHolder;


    private static int _earnedForRun;

    public event Action<GameStates> OnGameStateChanged;


    public static int EarnedForRun { get => _earnedForRun; private set => _earnedForRun = value; }
    public static UpgradeValueHolder ValueHolder { get => _valueHolder; private set => _valueHolder = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }   
        else
        {
            Destroy(gameObject);

        }


        InitSave();

    }





    public static void SaveData()
    {
        SaveInitiator.SaveValues(ValueHolder);

    }

    private void InitSave()
    {
        var values = SaveInitiator.GetValues();







        if (values != null)
        {
            ValueHolder = values;


        }
        else
        {
            Debug.Log("Cant find save , creating new");
            ValueHolder = new UpgradeValueHolder();






            ValueHolder.AddEnums();


        }

        foreach (var slot in ShopItems)
        {
            if (!ValueHolder.UpgradesLevels.ContainsKey(slot))
                ValueHolder.UpgradesLevels.Add(slot, 0);
        }






        ValueHolder.Points += 999;
    }


    public static void AddPoints(int point)
    {
        EarnedForRun += point;
        ValueHolder.Points += point;
    }


    private void LevelStart()
    {
        gameState = GameStates.countDown;
        EarnedForRun = 0;
        HealthBar.Instance.SetBloodHealth(BloodManager.Instance.MaxbloodAmount);
        Debug.Log("Level started");
    }


    public GameStates GetGameState()
    {
        return gameState;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;


            OnGameStateChanged?.Invoke(gameState);
        }
    }

    public float GetRaceTime()
    {
        switch (gameState)
        {
            case GameStates.countDown:
            return 0;

            case GameStates.raceOver:
            return raceCompletedTime - raceStartedTime;


            default:
            return Time.time - raceStartedTime;
        }


    }


    public void RestartRace()
    {
        if (gameState == GameStates.raceOver) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }






    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");
        BloodManager.Instance.StartDrain();
        raceStartedTime = Time.time;

        ChangeGameState(GameStates.running);
    }
    public void OnRaceCompleted()
    {
        CarInputHandler.Instance.isUIInput = true;
        BloodManager.Instance.StopDrain();
        Debug.Log("OnRaceCompleted");

        raceCompletedTime = Time.time;

        ShopManager.RefreshPointsText();

        ChangeGameState(GameStates.raceOver);

        SaveData();
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();

    }

}
