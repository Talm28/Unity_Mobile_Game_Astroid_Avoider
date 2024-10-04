using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _forchMagnitude;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _rotationSpeed;

    private Camera _mainCamera;
    private Rigidbody _rigidBody;

    private Vector3 _movementDirection;

    void Start()
    {
        _mainCamera = Camera.main;  
        _rigidBody = GetComponent<Rigidbody>();  
    }

    void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();

        RotatePlayer();
    }

    void FixedUpdate() 
    {
        if(_movementDirection == Vector3.zero)
            return;
        
        _rigidBody.AddForce(_movementDirection * _forchMagnitude * Time.deltaTime, ForceMode.Force);

        _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _maxVelocity);
    }

    private void RotatePlayer()
    {
        if(_rigidBody.velocity == Vector3.zero)
            return;
        
        Quaternion targerRotation = Quaternion.LookRotation(_rigidBody.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, targerRotation, _rotationSpeed * Time.deltaTime);
    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(transform.position);

        if(viewportPosition.x > 1)
            newPosition.x = -newPosition.x + 0.1f;
        else if(viewportPosition.x < 0)
            newPosition.x = -newPosition.x - 0.1f;
        else if(viewportPosition.y < 0)
            newPosition.y = -newPosition.y - 0.1f;
        else if(viewportPosition.y > 1)
            newPosition.y = -newPosition.y + 0.1f;

        transform.position = newPosition;
    }

    private void ProcessInput()
    {
        Vector3? touchInput = GetTouchInput();
        if (touchInput != null)
        {
            _movementDirection = (Vector3)touchInput - transform.position;
            _movementDirection.z = 0;
            _movementDirection.Normalize();
        }
        else
        {
            _movementDirection = Vector3.zero;
        }
    }

    private Vector3? GetTouchInput()
    {
        if(Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentTouch = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(currentTouch);
            return worldPosition;
        }
        return null;
    }
}
