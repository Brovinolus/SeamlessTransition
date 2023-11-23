using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    private PlayerInputActions _playerInputActions;
    private Camera _mainCamera;

    private void Awake() 
    {
        _playerInputActions = new PlayerInputActions();
        _mainCamera = Camera.main;
        
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() 
    {
        _playerInputActions.Enable();
        _playerInputActions.Player.Click.performed += Clicked;
    }

    private void OnDisable() 
    {
        _playerInputActions.Disable();
        _playerInputActions.Player.Click.performed -= Clicked;
    }

    private void Clicked(InputAction.CallbackContext context)
    {
        CheckForInteractableOnClick(Mouse.current.position.ReadValue());
    }
    
    private void CheckForInteractableOnClick(Vector2 screenPosition) 
    {
        if (_mainCamera == null) // script might lose the reference on scene change
            _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.Log("No Camera on the scene");
            return;
        }

        var ray = _mainCamera.ScreenPointToRay(screenPosition);
        
        if (!Physics.Raycast(ray, out var hit, 100)) return;
        var interactComponent = hit.collider.GetComponent<IInteract>();
        interactComponent?.Interact();
    }
}
