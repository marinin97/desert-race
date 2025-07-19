using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public static event Action<int> OnGasolineAdded;
    public static event Action OnCarDestroyed;
    public float playerSpeed;
    public float maxTurnAngle;
    public float turnSpeed;
    public float maxPlayerSpeed;

    private float borderPositionX = 12;
    private Quaternion _targetRotation;
    private Quaternion initialRotation;
    private Vector3 _direction;
    private float _friction = 0.99f;
    private bool _carPaused = false;

    private void Start()
    {
        initialRotation = transform.rotation;
        PauseManager.OnGamePaused += CarPaused;
        PauseManager.OnGameResumed += CarResumed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("barrel"))
        {
            var barrelRender = other.GetComponent<Renderer>();
            barrelRender.enabled = false;
            int addedBarrel = 1;
            OnGasolineAdded?.Invoke(addedBarrel);
        }
        if (other.CompareTag("car"))
        {
            OnCarDestroyed?.Invoke();
            turnSpeed = 0;
            playerSpeed = 0;
        }
    }
        private void FixedUpdate()
        {
            if (!_carPaused)
            {
                _direction *= 1 - _friction * Time.deltaTime;
                _direction += Vector3.right * playerSpeed * Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime;
                _direction = Vector3.ClampMagnitude(_direction, maxPlayerSpeed);
                _direction = Vector3.Lerp(_direction.normalized, Vector3.forward, 1 * Time.fixedDeltaTime) * _direction.magnitude;
                transform.position += _direction;
            }
        }
        private void Update()
        {
            float playerPosition = Mathf.Clamp(transform.position.x, -borderPositionX, borderPositionX);

            transform.position = new Vector3(playerPosition, transform.position.y, 0);

            if (!_carPaused)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    _targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - maxTurnAngle, 0);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    _targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + maxTurnAngle, 0);
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    _targetRotation = initialRotation;
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    _targetRotation = initialRotation;
                }
            }


        }
        private void LateUpdate()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * turnSpeed);

        }
        private void CarPaused()
        {
            _carPaused = true;
        }
        private void CarResumed()
        {
            _carPaused = false;
        }
        private void OnDestroy()
        {
            PauseManager.OnGamePaused -= CarPaused;
            PauseManager.OnGameResumed -= CarResumed;
        }
    }
