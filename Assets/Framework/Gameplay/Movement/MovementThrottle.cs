using System;
using UnityEngine;

[RequireComponent(typeof(MovementStats))]
public class MovementThrottle : MonoBehaviour
{
    public MovementThrottleLevel CurrentLevel { get; private set; }
        = MovementThrottleLevel.Stopped;

    public MovementThrottleLevel RequestedLevel { get; private set; }
        = MovementThrottleLevel.Stopped;

    public bool IsChanging { get; private set; }

    public float RemainingTransitionTime { get; private set; }

    public event Action<MovementThrottleLevel> GearChangeStarted;
    public event Action<MovementThrottleLevel> GearChangeCompleted;

    private MovementStats stats;

    private void Awake()
    {
        stats = GetComponent<MovementStats>();
    }

    private void Update()
    {
        if (!IsChanging)
            return;

        RemainingTransitionTime -= Time.deltaTime;

        if (RemainingTransitionTime <= 0f)
        {
            CompleteGearChange();
        }
    }

    public void Increase()
    {
        if (IsChanging)
            return;

        MovementThrottleLevel next =
            (MovementThrottleLevel)Mathf.Min(
                (int)CurrentLevel + 1,
                (int)MovementThrottleLevel.FullSails
            );

        TrySetLevel(next);
    }

    public void Decrease()
    {
        if (IsChanging)
            return;

        MovementThrottleLevel next =
            (MovementThrottleLevel)Mathf.Max(
                (int)CurrentLevel - 1,
                (int)MovementThrottleLevel.Reverse
            );

        TrySetLevel(next);
    }

    public void Stop()
    {
        TrySetLevel(MovementThrottleLevel.Stopped);
    }

    public bool TrySetLevel(MovementThrottleLevel newLevel)
    {
        if (IsChanging)
            return false;

        if (CurrentLevel == newLevel)
            return true;

        if (!stats.IsGearAvailable(newLevel))
        {
            Debug.Log(
                $"{name}: Gear {newLevel} is unavailable.",
                this
            );

            return false;
        }

        ShipSailController sailController =
            GetComponent<ShipSailController>();

        if (sailController != null &&
            !sailController.CanUseSailLevel(newLevel))
        {
            Debug.Log(
                $"{name}: Not enough available sails for {newLevel}.",
                this
            );

            return false;
        }

        RequestedLevel = newLevel;

        float transitionTime =
            GetTransitionTime(CurrentLevel, newLevel);

        IsChanging = true;
        RemainingTransitionTime = transitionTime;

        GearChangeStarted?.Invoke(newLevel);

        if (transitionTime <= 0f)
            CompleteGearChange();

        return true;
    }

    public void SetLevel(MovementThrottleLevel newLevel)
    {
        TrySetLevel(newLevel);
    }

    private void CompleteGearChange()
    {
        CurrentLevel = RequestedLevel;

        IsChanging = false;
        RemainingTransitionTime = 0f;

        GearChangeCompleted?.Invoke(CurrentLevel);
    }

    private float GetTransitionTime(
        MovementThrottleLevel from,
        MovementThrottleLevel to)
    {
        if (stats.BaseSettings == null)
            return 0f;

        return stats.BaseSettings.GetTransitionTime(from, to);
    }

    public bool IsGearAvailable(MovementThrottleLevel level)
    {
        return stats != null &&
               stats.IsGearAvailable(level);
    }
}