using UnityEngine;

public readonly struct PlayerDiedEvent
{
}

public readonly struct EntityDiedEvent
{
    public readonly GameObject Entity;

    public EntityDiedEvent(GameObject entity)
    {
        Entity = entity;
    }
}

public readonly struct DamageTakenEvent
{
    public readonly GameObject Target;
    public readonly float Amount;

    public DamageTakenEvent(
        GameObject target,
        float amount)
    {
        Target = target;
        Amount = amount;
    }
}
public readonly struct GameStateChangedEvent
{
    public readonly GameState State;

    public GameStateChangedEvent(GameState state)
    {
        State = state;
    }
}