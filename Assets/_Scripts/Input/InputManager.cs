using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Action<Vector2> OnPress;

    private UserInputActions userInput;
    private Vector2 screenPosition;


    private void Awake()
    {
        userInput = new UserInputActions();
        userInput.Enable();
        
    }

    private void OnEnable()
    {
        userInput.Player.ScreenPosition.performed += UpdateScreenPosition;
        userInput.Player.Press.performed += HandlePress;
    }
    private void OnDisable()
    {
        userInput.Player.Press.performed -= HandlePress;
        userInput.Player.ScreenPosition.performed -= UpdateScreenPosition;
    }

    private void UpdateScreenPosition(InputAction.CallbackContext ctx)
    {
        screenPosition = ctx.ReadValue<Vector2>();
    }

    private void HandlePress(InputAction.CallbackContext ctx)
    {
        if (screenPosition != null)
        {
            OnPress?.Invoke(screenPosition);
        }
    }

}
