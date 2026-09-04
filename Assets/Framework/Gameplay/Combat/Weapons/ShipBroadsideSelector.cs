using UnityEngine;
using UnityEngine.InputSystem;

public class ShipBroadsideSelector : MonoBehaviour
{
    [SerializeField]
    private Camera gameplayCamera;

    [SerializeField]
    private Transform ship;

    public WeaponSide GetSideFromMouse()
    {
        if (gameplayCamera == null)
            return WeaponSide.Left;

        if (ship == null)
            return WeaponSide.Left;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            gameplayCamera.ScreenPointToRay(mousePosition);

        Plane waterPlane =
            new Plane(Vector3.up, ship.position);

        if (!waterPlane.Raycast(ray, out float distance))
        {
            return WeaponSide.Left;
        }

        Vector3 mouseWorldPosition =
            ray.GetPoint(distance);

        Vector3 localPosition =
            ship.InverseTransformPoint(mouseWorldPosition);

        if (localPosition.x < 0f)
        {
            return WeaponSide.Left;
        }

        return WeaponSide.Right;
    }
}