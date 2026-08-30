using UnityEngine;

public static class GameTime
{
    public static float DeltaTime =>
        UnityEngine.Time.deltaTime;

    public static float UnscaledDeltaTime =>
        UnityEngine.Time.unscaledDeltaTime;

    public static float Time =>
        UnityEngine.Time.time;
}