using UnityEngine;
using TMPro;

public class GasolineCounter : MonoBehaviour
{
    public TMP_Text CurrentGasolineText;
    public float CurrentGasoline;
    public GameObject BarrelCanvasPrefab;
    public Transform BarrelSpawnPointCanvas;

    private float _timeToDecrease = 3f;
    private bool _stopCount;
    private bool _counter = true;

    public void Awake()
    {
        PauseManager.OnGamePaused += PauseCounter;
        PauseManager.OnGameResumed += ResumeCounter;
        PlayerController.OnGasolineAdded += AddedGasolineHandler;
        PlayerController.OnGasolineAdded += ShowAddedScoreAnimation;
        PlayerController.OnCarDestroyed += PauseCounter;
    }

    private void Update()
    {
        CurrentGasolineText.text = $"Gasoline: {(int)CurrentGasoline:D3}l";
        if (_counter)
        {
            if (!_stopCount)
            {
                _timeToDecrease -= Time.deltaTime;
                if (_timeToDecrease <= 0)
                {
                    CurrentGasoline--;
                    _timeToDecrease = 3f;
                    if (CurrentGasoline == 0)
                    {
                        print("GameOver");
                        _stopCount = true;
                    }
                }
            }
        }

    }
    private void AddedGasolineHandler(int addedBarrel)
    {
        CurrentGasoline += addedBarrel;

        ShowAddedScoreAnimation(1);
    }
    private void PauseCounter()
    {
        _counter = false;
    }
    private void ResumeCounter()
    {
        _counter = true;
    }

    private void OnDestroy()
    {
        PlayerController.OnGasolineAdded -= AddedGasolineHandler;
        PauseManager.OnGamePaused -= PauseCounter;
        PauseManager.OnGameResumed -= ResumeCounter;
        PlayerController.OnCarDestroyed -= PauseCounter;
    }

    private void ShowAddedScoreAnimation(int i)
    {
        var canvas = Instantiate(BarrelCanvasPrefab);
        var spawnPoint = canvas.transform.GetChild(0);
        var pointOnScreen = Camera.main.WorldToScreenPoint(BarrelSpawnPointCanvas.position);
        spawnPoint.position = pointOnScreen;
        var animationText = canvas.GetComponentInChildren<BarrelTextAnimation>();
        animationText.ShowAnimation();
        Destroy(canvas, 1f);
    }

}
