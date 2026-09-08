using UnityEngine;

[RequireComponent(typeof(MovementThrottle))]
public class ShipSailController : MonoBehaviour
{
    [Header("Sails")]

    [SerializeField]
    private ShipSail[] sails;

    [Header("Deployment Rules")]

    [Min(0)]
    [SerializeField]
    private int sailsForPartialSails = 2;

    [Min(0)]
    [SerializeField]
    private int sailsForFullSails = 4;

    private MovementThrottle throttle;

    private void Awake()
    {
        throttle = GetComponent<MovementThrottle>();
    }

    private void OnEnable()
    {
        if (throttle == null)
            throttle = GetComponent<MovementThrottle>();

        throttle.GearChangeCompleted += OnGearChangeCompleted;
    }

    private void OnDisable()
    {
        if (throttle != null)
            throttle.GearChangeCompleted -= OnGearChangeCompleted;
    }

    private void Start()
    {
        ApplySailState(throttle.CurrentLevel);
    }

    private void OnGearChangeCompleted(
        MovementThrottleLevel newLevel)
    {
        ApplySailState(newLevel);
    }

    public void ApplySailState(MovementThrottleLevel level)
    {
        int requiredSails = GetRequiredSailCount(level);

        int deployedCount = 0;

        for (int i = 0; i < sails.Length; i++)
        {
            if (sails[i] == null)
                continue;

            bool shouldDeploy =
                sails[i].IsAvailable &&
                deployedCount < requiredSails;

            sails[i].SetDeployed(shouldDeploy);

            if (shouldDeploy)
                deployedCount++;
        }
    }

    private int GetRequiredSailCount(
        MovementThrottleLevel level)
    {
        switch (level)
        {
            case MovementThrottleLevel.PartialSails:
                return sailsForPartialSails;

            case MovementThrottleLevel.FullSails:
                return sailsForFullSails;

            default:
                return 0;
        }
    }

    public int GetAvailableSailCount()
    {
        int count = 0;

        foreach (ShipSail sail in sails)
        {
            if (sail != null && sail.IsAvailable)
                count++;
        }

        return count;
    }

    public int GetDeployedSailCount()
    {
        int count = 0;

        foreach (ShipSail sail in sails)
        {
            if (sail != null && sail.IsDeployed)
                count++;
        }

        return count;
    }

    public void SetSailAvailability(
        int sailIndex,
        bool available)
    {
        if (sailIndex < 0 || sailIndex >= sails.Length)
            return;

        if (sails[sailIndex] == null)
            return;

        sails[sailIndex].SetAvailable(available);

        ApplySailState(throttle.CurrentLevel);
    }
    public bool CanUseSailLevel(
    MovementThrottleLevel level)
    {
        if (level != MovementThrottleLevel.PartialSails &&
            level != MovementThrottleLevel.FullSails)
        {
            return true;
        }

        int availableSails = GetAvailableSailCount();

        if (level == MovementThrottleLevel.PartialSails)
            return availableSails >= sailsForPartialSails;

        return availableSails >= sailsForFullSails;
    }
}