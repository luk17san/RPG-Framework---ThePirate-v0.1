using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> events = new();

    public static void Subscribe<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (events.TryGetValue(type, out var existing))
        {
            events[type] = Delegate.Combine(existing, listener);
        }
        else
        {
            events[type] = listener;
        }
    }

    public static void Unsubscribe<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (!events.TryGetValue(type, out var existing))
            return;

        var updated = Delegate.Remove(existing, listener);

        if (updated == null)
            events.Remove(type);
        else
            events[type] = updated;
    }

    public static void Publish<T>(T eventData)
    {
        var type = typeof(T);

        if (!events.TryGetValue(type, out var existing))
            return;

        if (existing is Action<T> callback)
        {
            callback.Invoke(eventData);
        }
    }

    public static void Clear()
    {
        events.Clear();
    }
}