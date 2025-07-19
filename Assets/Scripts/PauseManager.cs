using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public Button PauseGameButton;
    public Button ResumeGameButton;
    public Button ExitGameButton;
    public GameObject PausePanel;

    public void Awake()
    {
        PauseGameButton.onClick.AddListener(PauseGame);
        ResumeGameButton.onClick.AddListener(ResumeGame);
        ExitGameButton.onClick.AddListener(ExitGame);
        PlayerController.OnCarDestroyed += DeactivatePauseButton;
    }

    private void PauseGame()
    {
        OnGamePaused?.Invoke();
        PauseGameButton.interactable = false;
        PausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        OnGameResumed?.Invoke();
        PauseGameButton.interactable = true;
        PausePanel.SetActive(false);
    }

    private void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    private void DeactivatePauseButton()
    {
        PauseGameButton.interactable = false;
    }

    public void OnDestroy()
    {
        PlayerController.OnCarDestroyed -= DeactivatePauseButton;
    }
}
