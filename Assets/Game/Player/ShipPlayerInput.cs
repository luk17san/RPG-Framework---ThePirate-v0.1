using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ShipController))]
public class ShipPlayerInput : MonoBehaviour, IMovementInput
{
    public float Turn { get; private set; }

    private GameInputActions input;
    private ShipController shipController;

    private void Awake()
    {
        input = new GameInputActions();
        shipController = GetComponent<ShipController>();
    }

    private void OnEnable()
    {
        input.Ship.Enable();
        input.Ship.ThrottleUp.performed += OnThrottleUp;
        input.Ship.ThrottleDown.performed += OnThrottleDown;
        input.Ship.Stop.performed += OnStop;
    }

    private void OnDisable()
    {
        input.Ship.ThrottleUp.performed -= OnThrottleUp;
        input.Ship.ThrottleDown.performed -= OnThrottleDown;
        input.Ship.Stop.performed -= OnStop;
        input.Ship.Disable();
    }

    private void Update()
    {
        Turn = input.Ship.Steer.ReadValue<float>();
    }

    private void OnThrottleUp(InputAction.CallbackContext context)
    {
        shipController.Throttle.Increase();
    }

    private void OnThrottleDown(InputAction.CallbackContext context)
    {
        shipController.Throttle.Decrease();
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        shipController.Throttle.Stop();
    }
}
