using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponController : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField]
    private List<Weapon> weapons = new();

    public void FireWeaponType(WeaponType weaponType)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon == null)
                continue;

            if (weapon.Definition == null)
                continue;

            if (weapon.Definition.weaponType != weaponType)
                continue;

            weapon.Fire();
        }
    }
}