using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuUIHandler : MonoBehaviour
{

    private Canvas canvas;


    [SerializeField]
    private TextMeshProUGUI _earnedText;

    public TextMeshProUGUI EarnedText { get => _earnedText; }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        canvas.enabled = false;

    
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    // Вызов кнопкой, не удаляй
    public void RequestRestart()
    {
        GameManager.instance.RestartRace();
    }

   
    public void RefreshEarned()
    {
        EarnedText.text = string.Format("За заезд получено : {0}", GameManager.EarnedForRun);
    }


    IEnumerator ShowMenuCO()
    {
        yield return new WaitForSeconds(1);

        canvas.enabled = true;
    }

    private void OnGameStateChanged(GameStates state)
    {
        if (state == GameStates.raceOver)
        {
            RefreshEarned();
            StartCoroutine(ShowMenuCO());
        }
    }

    void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

}
