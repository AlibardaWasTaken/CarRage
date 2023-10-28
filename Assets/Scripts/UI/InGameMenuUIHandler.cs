using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class InGameMenuUIHandler : MonoBehaviour
{
    //Other components
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        canvas.enabled = false;

        //Hook up events
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }


    public void OnRaceAgain()
    {
      
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    IEnumerator ShowMenuCO()
    {
        yield return new WaitForSeconds(1);

        canvas.enabled = true;
    }

    //Events 
    void OnGameStateChanged(GameManager gameManager)
    {
        if (GameManager.instance.GetGameState() == GameStates.raceOver)
        {
            StartCoroutine(ShowMenuCO());
        }
    }

    void OnDestroy()
    {
        //Unhook events
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

}
