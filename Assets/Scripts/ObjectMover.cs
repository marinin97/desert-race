using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Transform _transform;


    public void Awake()
    {
        PauseManager.OnGamePaused += PauseMove;
        PauseManager.OnGameResumed += ResumeMove;
        PlayerController.OnCarDestroyed += PauseMove;
    }
    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _transform.position += Vector3.back * _speed * Time.deltaTime;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    private void PauseMove()
    {
        _speed = 0;
    }
    private void ResumeMove()
    {
        _speed = 60;
    }
    public void OnDestroy()
    {
        PauseManager.OnGamePaused -= PauseMove;
        PauseManager.OnGameResumed -= ResumeMove;
        PlayerController.OnCarDestroyed -= PauseMove;
    }

}
