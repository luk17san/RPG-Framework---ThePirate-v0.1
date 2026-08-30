using UnityEngine;

[RequireComponent(typeof(MovementThrottle))]
[RequireComponent(typeof(MovementStats))]
[RequireComponent(typeof(MovementController))]
public class ShipController : MonoBehaviour
{
    public MovementThrottle Throttle { get; private set; }
    public MovementStats MovementStats { get; private set; }
    public MovementController MovementController { get; private set; }

    private void Awake()
    {
        Throttle = GetComponent<MovementThrottle>();
        MovementStats = GetComponent<MovementStats>();
        MovementController = GetComponent<MovementController>();
    }
}
