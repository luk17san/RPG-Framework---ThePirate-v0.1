using UnityEngine;

[System.Serializable]
public class MovementGearSettings
{
    [SerializeField] private bool available = true;

    [Min(0f)]
    [SerializeField] private float speed = 0f;

    [Min(0f)]
    [SerializeField] private float acceleration = 2f;

    [Min(0f)]
    [SerializeField] private float deceleration = 3.5f;

    [Range(0f, 2f)]
    [SerializeField] private float turnMultiplier = 1f;

    public bool Available => available;
    public float Speed => speed;
    public float Acceleration => acceleration;
    public float Deceleration => deceleration;
    public float TurnMultiplier => turnMultiplier;
}

[CreateAssetMenu(
    fileName = "MovementSettings",
    menuName = "Framework/Movement/Movement Settings"
)]
public class MovementSettings : ScriptableObject
{
    [Header("Gear Settings")]

    [SerializeField]
    private MovementGearSettings reverse = new MovementGearSettings();

    [SerializeField]
    private MovementGearSettings stopped = new MovementGearSettings();

    [SerializeField]
    private MovementGearSettings oars = new MovementGearSettings();

    [SerializeField]
    private MovementGearSettings partialSails = new MovementGearSettings();

    [SerializeField]
    private MovementGearSettings fullSails = new MovementGearSettings();

    [Header("Steering")]

    [Min(0f)]
    [SerializeField]
    private float turnSpeed = 45f;

    [Min(0f)]
    [SerializeField]
    private float steeringResponse = 4f;

    [Range(0f, 1f)]
    [SerializeField]
    private float turnAuthorityAtRest = 0.15f;

    [Header("Gear Transition")]

    [Min(0f)]
    [SerializeField]
    private float gearChangeCooldown = 0.5f;

    public MovementGearSettings Reverse => reverse;
    public MovementGearSettings Stopped => stopped;
    public MovementGearSettings Oars => oars;
    public MovementGearSettings PartialSails => partialSails;
    public MovementGearSettings FullSails => fullSails;

    [Header("Gear Transition Times")]

    [Min(0f)]
    [SerializeField]
    private float sailRaiseTime = 1.5f;

    [Min(0f)]
    [SerializeField]
    private float sailLowerTime = 1.5f;

    [Min(0f)]
    [SerializeField]
    private float gearChangeTime = 0.5f;

    public float SailRaiseTime => sailRaiseTime;
    public float SailLowerTime => sailLowerTime;
    public float GearChangeTime => gearChangeTime;
    public float TurnSpeed => turnSpeed;
    public float SteeringResponse => steeringResponse;
    public float TurnAuthorityAtRest => turnAuthorityAtRest;
    public float GearChangeCooldown => gearChangeCooldown;

    public float GetTransitionTime(
    MovementThrottleLevel from,
    MovementThrottleLevel to)
    {
        bool fromUsesSails =
            from == MovementThrottleLevel.PartialSails ||
            from == MovementThrottleLevel.FullSails;

        bool toUsesSails =
            to == MovementThrottleLevel.PartialSails ||
            to == MovementThrottleLevel.FullSails;

        if (!fromUsesSails && toUsesSails)
            return sailRaiseTime;

        if (fromUsesSails && !toUsesSails)
            return sailLowerTime;

        return gearChangeTime;
    }
    public MovementGearSettings GetGear(MovementThrottleLevel level)
    {
        switch (level)
        {
            case MovementThrottleLevel.Reverse:
                return reverse;

            case MovementThrottleLevel.Stopped:
                return stopped;

            case MovementThrottleLevel.Oars:
                return oars;

            case MovementThrottleLevel.PartialSails:
                return partialSails;

            case MovementThrottleLevel.FullSails:
                return fullSails;

            default:
                return stopped;
        }
    }
}