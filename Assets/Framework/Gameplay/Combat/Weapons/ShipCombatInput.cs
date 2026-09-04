using UnityEngine;
using UnityEngine.InputSystem;

public class ShipCombatInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private ShipWeaponController weaponController;

    [SerializeField]
    private ShipBroadsideSelector broadsideSelector;

    [Header("Input")]
    [SerializeField]
    private InputAction fireLight;

    [SerializeField]
    private InputAction fireHeavy;

    private void OnEnable()
    {
        fireLight.Enable();
        fireHeavy.Enable();

        fireLight.performed += OnFireLight;
        fireHeavy.performed += OnFireHeavy;
    }

    private void OnDisable()
    {
        fireLight.performed -= OnFireLight;
        fireHeavy.performed -= OnFireHeavy;

        fireLight.Disable();
        fireHeavy.Disable();
    }

    private void OnFireLight(
        InputAction.CallbackContext context)
    {
        FireBroadside(WeaponType.LightCannons);
    }

    private void OnFireHeavy(
        InputAction.CallbackContext context)
    {
        FireBroadside(WeaponType.HeavyCannons);
    }

    private void FireBroadside(
        WeaponType weaponType)
    {
        if (weaponController == null)
            return;

        if (broadsideSelector == null)
            return;

        WeaponSide side =
            broadsideSelector.GetSideFromMouse();

        weaponController.FireBroadside(
            weaponType,
            side
        );
    }
}