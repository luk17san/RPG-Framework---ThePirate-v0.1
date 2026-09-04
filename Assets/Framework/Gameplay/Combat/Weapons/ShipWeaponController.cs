using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponController : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField]
    private List<Weapon> weapons = new();

    [Header("Broadside")]
    [SerializeField]
    private BroadsideSequence broadsideSequence;

    [SerializeField]
    private BroadsideCooldown broadsideCooldown;

    public bool IsBroadsideReady =>
        broadsideCooldown != null &&
        broadsideCooldown.IsReady;

    public bool IsBroadsideFiring =>
        broadsideSequence != null &&
        broadsideSequence.IsFiring;

    public float BroadsideRemainingTime =>
        broadsideCooldown != null
            ? broadsideCooldown.RemainingTime
            : 0f;

    public float BroadsideCooldownProgress =>
        broadsideCooldown != null
            ? broadsideCooldown.NormalizedRemainingTime
            : 0f;

    private void Awake()
    {
        if (broadsideSequence == null)
        {
            broadsideSequence =
                GetComponent<BroadsideSequence>();
        }

        if (broadsideCooldown == null)
        {
            broadsideCooldown =
                GetComponent<BroadsideCooldown>();
        }
    }

    public bool FireBroadside(
        WeaponType weaponType,
        WeaponSide side)
    {
        if (broadsideSequence == null)
        {
            Debug.LogWarning(
                $"{name}: BroadsideSequence is missing."
            );

            return false;
        }

        if (broadsideCooldown == null)
        {
            Debug.LogWarning(
                $"{name}: BroadsideCooldown is missing."
            );

            return false;
        }

        if (!broadsideCooldown.IsReady)
            return false;

        List<Weapon> selectedWeapons = new();

        foreach (Weapon weapon in weapons)
        {
            if (weapon == null)
                continue;

            if (weapon.Definition == null)
                continue;

            if (weapon.Definition.weaponType != weaponType)
                continue;

            if (weapon.Side != side)
                continue;

            selectedWeapons.Add(weapon);
        }

        if (selectedWeapons.Count == 0)
            return false;

        if (!broadsideCooldown.TryStartCooldown())
            return false;

        broadsideSequence.FireSequence(
            selectedWeapons,
            OnSequenceCompleted
        );

        return true;
    }

    private void OnSequenceCompleted()
    {
        // Sekwencja zakoñczona.
        // Cooldown nadal trwa.
    }

    public void ResetBroadsideCooldown()
    {
        if (broadsideCooldown != null)
        {
            broadsideCooldown.ResetCooldown();
        }
    }
}