using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
public static InputManager instance;

public bool MenuOpenInput { get; private set;}
public bool MenuCloseInput { get; private set;}

public static PlayerInput _playerInput;
private InputAction _MenuOpenAction;
private InputAction _MenuCloseAction;


private void Awake()
{
    if (instance == null)
    {
        instance = this;
    }
    _playerInput = GetComponent<PlayerInput>();
    _MenuOpenAction = _playerInput.actions["MenuOpen"];
    _MenuCloseAction = _playerInput.actions["MenuClose"];
}


    void Update()
    {
        MenuOpenInput = _MenuOpenAction.WasPressedThisFrame();
        MenuCloseInput = _MenuCloseAction.WasPressedThisFrame();
    }
}
