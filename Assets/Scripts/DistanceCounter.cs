using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DistanceCounter : MonoBehaviour
{
    public TMP_Text DistanceText;

    private float _distance;
    private float _distanceMultiplier;
    private float _bestDistance;
    private float _timeToAddDistance = 0.5f;
    private bool _count = true;

    public void Awake()
    {
        PauseManager.OnGamePaused += PauseCount;
        PauseManager.OnGameResumed += ResumeCount;
        PlayerController.OnCarDestroyed += PauseCount;
        _bestDistance = PlayerPrefs.GetFloat("distance", 0);
    }

    public void Update()
    {
        if (_count)
        {
            _timeToAddDistance -= Time.deltaTime;
            if (_timeToAddDistance <= 0)
            {
                _distanceMultiplier = Random.Range(1, 5);
                _distance += _distanceMultiplier;

                if (_distance > _bestDistance)
                {
                    _bestDistance = _distance;
                    PlayerPrefs.SetFloat("distance", _bestDistance);
                    PlayerPrefs.Save();
                }
                DistanceText.text = $"Distance: {(int)_distance:D6}m";
                _timeToAddDistance = 0.5f;
            }
        }
        
    }

    public void ShowDistance(TMP_Text currentScoreText, TMP_Text bestScoreText)
    {
        currentScoreText.text = $"Current Score: {_distance}";
        bestScoreText.text = $"Best Score: {_bestDistance}";
        DistanceText.gameObject.SetActive(false);
    }
    
    private void PauseCount()
    {
        _count = false;
    }
    private void ResumeCount()
    {
        _count = true;
    }
    public void OnDestroy()
    {
        PauseManager.OnGamePaused -= PauseCount;
        PauseManager.OnGameResumed -= ResumeCount;
        PlayerController.OnCarDestroyed -= PauseCount;
    }
}
