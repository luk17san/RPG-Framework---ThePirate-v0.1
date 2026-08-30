using System;
using UnityEngine;

public class MovementThrottle : MonoBehaviour
{
    public MovementThrottleLevel CurrentLevel { get; private set; } =
        MovementThrottleLevel.Stopped;

    public event Action<MovementThrottleLevel> LevelChanged;

    public void Increase()
    {
        SetLevel((MovementThrottleLevel)Mathf.Min(
            (int)CurrentLevel + 1,
            (int)MovementThrottleLevel.FullSails
        ));
    }

    public void Decrease()
    {
        SetLevel((MovementThrottleLevel)Mathf.Max(
            (int)CurrentLevel - 1,
            (int)MovementThrottleLevel.Reverse
        ));
    }

    public void Stop()
    {
        SetLevel(MovementThrottleLevel.Stopped);
    }

    public void SetLevel(MovementThrottleLevel newLevel)
    {
        if (CurrentLevel == newLevel)
            return;

        CurrentLevel = newLevel;
        LevelChanged?.Invoke(CurrentLevel);
    }
}
