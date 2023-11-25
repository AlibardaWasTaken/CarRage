using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    //Static instance of GameManager so other scripts can access it
    public static GameManager instance = null;

    //States
    GameStates gameState = GameStates.countDown;

    //Time
   private float raceStartedTime = 0;
   private float raceCompletedTime = 0;



 


    public static UpgradeValueHolder ValueHolder;

    //Driver information
    private List<DriverInfo> driverInfoList = new List<DriverInfo>();

    //Events
    public event Action<GameManager> OnGameStateChanged;


   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitSave();
        }   
        else
        {
            Destroy(gameObject);

        }


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


            foreach (var slot in ShopManager.instance.Items)
            {
                ValueHolder.UpgradesLevels.Add(slot, 0);
            }

            foreach (var Upg in (UpgradeEnums[])Enum.GetValues(typeof(UpgradeEnums)))
            {
                ValueHolder.EnumsValuesDictionary.Add(Upg, 0);
            }
           
        }


        ValueHolder.Points += 99999;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        driverInfoList.Add(new DriverInfo(1, "P1", 0, false));

    }

    void LevelStart()
    {
        gameState = GameStates.countDown;
        HealthBar.Instance.SetBloodHealth(100);
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

            //Invoke game state change event
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
            return 0;
        else if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else return Time.time - raceStartedTime;
    }

    //Driver information handling
    public void ClearDriversList()
    {
        driverInfoList.Clear();
    }


    public void AddDriverToList(int playerNumber, string name, int carUniqueID, bool isAI)
    {
        driverInfoList.Add(new DriverInfo(playerNumber, name, carUniqueID, isAI));
    }

    public void SetDriversLastRacePosition(int playerNumber, int position)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);
        driverInfo.lastRacePosition = position;
    }

    public void AddPointsToChampionship(int playerNumber, int points)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);

        driverInfo.championshipPoints += points;
    }

    DriverInfo FindDriverInfo(int playerNumber)
    {
        foreach (DriverInfo driverInfo in driverInfoList)
        {
            if (playerNumber == driverInfo.playerNumber)
                return driverInfo;
        }

        Debug.LogError($"FindDriverInfoBasedOnDriverNumber failed to find driver for player number {playerNumber}");

        return null;
    }

    public List<DriverInfo> GetDriverList()
    {
        return driverInfoList;
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

        ChangeGameState(GameStates.raceOver);
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
