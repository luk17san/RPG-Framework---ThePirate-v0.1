using System.Collections.Generic;
using UnityEngine;

public enum MovementStat
{
    ReverseSpeed,
    OarsSpeed,
    PartialSailsSpeed,
    FullSailsSpeed,
    Acceleration,
    Deceleration,
    TurnSpeed,
    SteeringResponse,
    TurnAuthorityAtRest
}

public interface IMovementStatsModifier
{
    float Modify(MovementStat stat, float currentValue);
}

public class MovementStats : MonoBehaviour
{
    [SerializeField]
    private MovementSettings baseSettings;

    private readonly List<IMovementStatsModifier> modifiers =
        new List<IMovementStatsModifier>();

    public MovementSettings BaseSettings => baseSettings;

    public float ReverseSpeed =>
        GetModifiedValue(
            MovementStat.ReverseSpeed,
            GetGearSpeed(MovementThrottleLevel.Reverse)
        );

    public float OarsSpeed =>
        GetModifiedValue(
            MovementStat.OarsSpeed,
            GetGearSpeed(MovementThrottleLevel.Oars)
        );

    public float PartialSailsSpeed =>
        GetModifiedValue(
            MovementStat.PartialSailsSpeed,
            GetGearSpeed(MovementThrottleLevel.PartialSails)
        );

    public float FullSailsSpeed =>
        GetModifiedValue(
            MovementStat.FullSailsSpeed,
            GetGearSpeed(MovementThrottleLevel.FullSails)
        );

    public float Acceleration =>
        GetModifiedValue(
            MovementStat.Acceleration,
            GetCurrentGearAcceleration()
        );

    public float Deceleration =>
        GetModifiedValue(
            MovementStat.Deceleration,
            GetCurrentGearDeceleration()
        );

    public float TurnSpeed =>
        GetModifiedValue(
            MovementStat.TurnSpeed,
            baseSettings != null ? baseSettings.TurnSpeed : 0f
        );

    public float SteeringResponse =>
        GetModifiedValue(
            MovementStat.SteeringResponse,
            baseSettings != null ? baseSettings.SteeringResponse : 0f
        );

    public float TurnAuthorityAtRest =>
        Mathf.Clamp01(
            GetModifiedValue(
                MovementStat.TurnAuthorityAtRest,
                baseSettings != null
                    ? baseSettings.TurnAuthorityAtRest
                    : 0f
            )
        );

    private void Awake()
    {
        RefreshModifiers();
    }

    public void RefreshModifiers()
    {
        modifiers.Clear();

        foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
        {
            if (component is IMovementStatsModifier modifier)
                modifiers.Add(modifier);
        }
    }

    public bool IsGearAvailable(MovementThrottleLevel level)
    {
        if (baseSettings == null)
            return false;

        return baseSettings.GetGear(level).Available;
    }

    public float GetTargetSpeed(MovementThrottleLevel level)
    {
        if (baseSettings == null)
            return 0f;

        MovementGearSettings gear = baseSettings.GetGear(level);

        if (!gear.Available)
            return 0f;

        float speed = gear.Speed;

        switch (level)
        {
            case MovementThrottleLevel.Reverse:
                return GetModifiedValue(
                    MovementStat.ReverseSpeed,
                    -speed
                );

            case MovementThrottleLevel.Oars:
                return GetModifiedValue(
                    MovementStat.OarsSpeed,
                    speed
                );

            case MovementThrottleLevel.PartialSails:
                return GetModifiedValue(
                    MovementStat.PartialSailsSpeed,
                    speed
                );

            case MovementThrottleLevel.FullSails:
                return GetModifiedValue(
                    MovementStat.FullSailsSpeed,
                    speed
                );

            case MovementThrottleLevel.Stopped:
            default:
                return 0f;
        }
    }

    public float GetAcceleration(MovementThrottleLevel level)
    {
        if (baseSettings == null)
            return 0f;

        return GetModifiedValue(
            MovementStat.Acceleration,
            baseSettings.GetGear(level).Acceleration
        );
    }

    public float GetDeceleration(MovementThrottleLevel level)
    {
        if (baseSettings == null)
            return 0f;

        return GetModifiedValue(
            MovementStat.Deceleration,
            baseSettings.GetGear(level).Deceleration
        );
    }

    public float GetTurnMultiplier(MovementThrottleLevel level)
    {
        if (baseSettings == null)
            return 1f;

        return baseSettings.GetGear(level).TurnMultiplier;
    }

    private float GetGearSpeed(MovementThrottleLevel level)
    {
        if (baseSettings == null)
            return 0f;

        return baseSettings.GetGear(level).Speed;
    }

    private float GetCurrentGearAcceleration()
    {
        return baseSettings != null
            ? baseSettings.Oars.Acceleration
            : 0f;
    }

    private float GetCurrentGearDeceleration()
    {
        return baseSettings != null
            ? baseSettings.Oars.Deceleration
            : 0f;
    }

    private float GetModifiedValue(
        MovementStat stat,
        float baseValue
    )
    {
        float result = baseValue;

        foreach (IMovementStatsModifier modifier in modifiers)
            result = modifier.Modify(stat, result);

        return Mathf.Max(0f, result);
    }
}