using UnityEngine;
using UnityEngine.InputSystem;

public class ShipCombatInput : MonoBehaviour
{
    [SerializeField]
    private ShipWeaponController weaponController;

    [SerializeField]
    private InputActionReference fireLightAction;

    [SerializeField]
    private InputActionReference fireHeavyAction;

    private void Awake()
    {
        Debug.Log(
            $"ShipCombatInput Awake on: {name}. " +
            $"Controller: {weaponController != null}, " +
            $"FireLight: {fireLightAction != null}, " +
            $"FireHeavy: {fireHeavyAction != null}"
        );
    }

    private void OnEnable()
    {
        if (fireLightAction != null)
        {
            fireLightAction.action.performed += OnFireLight;
            fireLightAction.action.Enable();
        }

        if (fireHeavyAction != null)
        {
            fireHeavyAction.action.performed += OnFireHeavy;
            fireHeavyAction.action.Enable();
        }

        Debug.Log("ShipCombatInput enabled.");
    }

    private void OnDisable()
    {
        if (fireLightAction != null)
        {
            fireLightAction.action.performed -= OnFireLight;
            fireLightAction.action.Disable();
        }

        if (fireHeavyAction != null)
        {
            fireHeavyAction.action.performed -= OnFireHeavy;
            fireHeavyAction.action.Disable();
        }
    }

    private void OnFireLight(InputAction.CallbackContext context)
    {
        Debug.Log("LPM received by ShipCombatInput.");

        if (weaponController == null)
        {
            Debug.LogWarning("ShipCombatInput: Weapon Controller is missing.");
            return;
        }

        weaponController.FireWeaponType(WeaponType.LightCannons);
    }

    private void OnFireHeavy(InputAction.CallbackContext context)
    {
        Debug.Log("PPM received by ShipCombatInput.");

        if (weaponController == null)
        {
            Debug.LogWarning("ShipCombatInput: Weapon Controller is missing.");
            return;
        }

        weaponController.FireWeaponType(WeaponType.HeavyCannons);
    }
}