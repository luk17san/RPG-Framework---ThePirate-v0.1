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
    [SerializeField] private MovementSettings baseSettings;

    private readonly List<IMovementStatsModifier> modifiers =
        new List<IMovementStatsModifier>();

    public float ReverseSpeed => GetModifiedValue(MovementStat.ReverseSpeed, BaseReverseSpeed);
    public float OarsSpeed => GetModifiedValue(MovementStat.OarsSpeed, BaseOarsSpeed);
    public float PartialSailsSpeed => GetModifiedValue(MovementStat.PartialSailsSpeed, BasePartialSailsSpeed);
    public float FullSailsSpeed => GetModifiedValue(MovementStat.FullSailsSpeed, BaseFullSailsSpeed);
    public float Acceleration => GetModifiedValue(MovementStat.Acceleration, BaseAcceleration);
    public float Deceleration => GetModifiedValue(MovementStat.Deceleration, BaseDeceleration);
    public float TurnSpeed => GetModifiedValue(MovementStat.TurnSpeed, BaseTurnSpeed);
    public float SteeringResponse => GetModifiedValue(MovementStat.SteeringResponse, BaseSteeringResponse);
    public float TurnAuthorityAtRest => Mathf.Clamp01(GetModifiedValue(MovementStat.TurnAuthorityAtRest, BaseTurnAuthorityAtRest));

    private float BaseReverseSpeed => baseSettings != null ? baseSettings.ReverseSpeed : 0f;
    private float BaseOarsSpeed => baseSettings != null ? baseSettings.OarsSpeed : 0f;
    private float BasePartialSailsSpeed => baseSettings != null ? baseSettings.PartialSailsSpeed : 0f;
    private float BaseFullSailsSpeed => baseSettings != null ? baseSettings.FullSailsSpeed : 0f;
    private float BaseAcceleration => baseSettings != null ? baseSettings.Acceleration : 0f;
    private float BaseDeceleration => baseSettings != null ? baseSettings.Deceleration : 0f;
    private float BaseTurnSpeed => baseSettings != null ? baseSettings.TurnSpeed : 0f;
    private float BaseSteeringResponse => baseSettings != null ? baseSettings.SteeringResponse : 0f;
    private float BaseTurnAuthorityAtRest => baseSettings != null ? baseSettings.TurnAuthorityAtRest : 0f;

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

    public float GetTargetSpeed(MovementThrottleLevel level)
    {
        switch (level)
        {
            case MovementThrottleLevel.Reverse: return -ReverseSpeed;
            case MovementThrottleLevel.Oars: return OarsSpeed;
            case MovementThrottleLevel.PartialSails: return PartialSailsSpeed;
            case MovementThrottleLevel.FullSails: return FullSailsSpeed;
            default: return 0f;
        }
    }

    private float GetModifiedValue(MovementStat stat, float baseValue)
    {
        float result = baseValue;

        foreach (IMovementStatsModifier modifier in modifiers)
            result = modifier.Modify(stat, result);

        return Mathf.Max(0f, result);
    }
}
