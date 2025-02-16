using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IService
{
    public static PlayerInputActions playerInputActions;
    public static bool IsControllerConnected { get; private set; }

    public static InputDevices inputDevice = InputDevices.MOUSE_AND_KEYBOARD;
    public enum InputDevices { MOUSE_AND_KEYBOARD,GAMEPAD}
    private void Update()
    {
        // Detects if any controller is connected
        IsControllerConnected = Gamepad.current != null;

        Cursor.visible = inputDevice != InputDevices.GAMEPAD;
    }

    public void EnablePlayerInput(bool enable)
    {
        if (enable) playerInputActions.Player.Enable();
        else playerInputActions.Player.Disable();
    }
    public void EnableUiInput(bool enable)
    {
        if (enable) playerInputActions.UI.Enable();
        else playerInputActions.UI.Disable();
    }

    private void OnInputActionForDeviceDetermination(object obj, InputActionChange context)
    {
        if (context == InputActionChange.ActionPerformed)
        {
            InputAction receivedInputAction = (InputAction)obj;
            InputDevice lastDevice = receivedInputAction.activeControl.device;
            if (lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse")) inputDevice = InputDevices.MOUSE_AND_KEYBOARD;
            else inputDevice = InputDevices.GAMEPAD;
        }
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        RegisterSelfAsService();

        InputSystem.onActionChange += OnInputActionForDeviceDetermination;
    }

    public void RegisterSelfAsService()
    {
        ServiceLocator.Register<InputManager>(this);
    }
}
