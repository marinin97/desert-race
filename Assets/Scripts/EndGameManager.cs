using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public GameObject EndGamePanel;
    public Button RestartButton;
    public Button ExitButton;
    public TMP_Text CurrentScoreText;
    public TMP_Text RecordScoreText;
    public DistanceCounter DistanceCounter;

    public void Awake()
    {
        PlayerController.OnCarDestroyed += EndGame;
        RestartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        ExitButton.onClick.AddListener(() => Application.Quit());
    }

    private void EndGame()
    {
        DistanceCounter.ShowDistance(CurrentScoreText, RecordScoreText);
        EndGamePanel.SetActive(true);
    }

    public void OnDestroy()
    {
        PlayerController.OnCarDestroyed -= EndGame;
    }
}
