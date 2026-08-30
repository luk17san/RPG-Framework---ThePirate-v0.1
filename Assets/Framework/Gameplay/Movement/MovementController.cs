using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementThrottle))]
[RequireComponent(typeof(MovementStats))]
public class MovementController : MonoBehaviour
{
    private Rigidbody body;
    private MovementThrottle throttle;
    private MovementStats stats;
    private IMovementInput movementInput;

    private float currentForwardSpeed;
    private float currentTurn;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        throttle = GetComponent<MovementThrottle>();
        stats = GetComponent<MovementStats>();
        movementInput = GetComponent<IMovementInput>();

        if (movementInput == null)
        {
            Debug.LogError($"{name}: IMovementInput is missing.", this);
        }

        currentForwardSpeed = Vector3.Dot(body.linearVelocity, transform.forward);
    }

    private void FixedUpdate()
    {
        if (movementInput == null)
            return;

        UpdateSteering();
        UpdateSpeed();
        ApplyRotation();
        ApplyVelocity();
    }

    private void UpdateSteering()
    {
        float targetTurn = Mathf.Clamp(movementInput.Turn, -1f, 1f);

        currentTurn = Mathf.MoveTowards(
            currentTurn,
            targetTurn,
            stats.SteeringResponse * Time.fixedDeltaTime
        );
    }

    private void UpdateSpeed()
    {
        float targetSpeed = stats.GetTargetSpeed(throttle.CurrentLevel);

        bool isChangingDirection =
            currentForwardSpeed != 0f &&
            targetSpeed != 0f &&
            Mathf.Sign(currentForwardSpeed) != Mathf.Sign(targetSpeed);

        bool isIncreasingSpeed =
            Mathf.Abs(targetSpeed) > Mathf.Abs(currentForwardSpeed) &&
            !isChangingDirection;

        float rate = isIncreasingSpeed
            ? stats.Acceleration
            : stats.Deceleration;

        currentForwardSpeed = Mathf.MoveTowards(
            currentForwardSpeed,
            targetSpeed,
            rate * Time.fixedDeltaTime
        );
    }

    private void ApplyRotation()
    {
        float speedFactor = Mathf.InverseLerp(
            0f,
            Mathf.Max(stats.FullSailsSpeed, 0.01f),
            Mathf.Abs(currentForwardSpeed)
        );

        float turnAuthority = Mathf.Lerp(
            stats.TurnAuthorityAtRest,
            1f,
            speedFactor
        );

        float turnAmount =
            currentTurn *
            stats.TurnSpeed *
            turnAuthority *
            Time.fixedDeltaTime;

        body.MoveRotation(
            body.rotation * Quaternion.Euler(0f, turnAmount, 0f)
        );
    }

    private void ApplyVelocity()
    {
        Vector3 planarVelocity = transform.forward * currentForwardSpeed;

        body.linearVelocity = new Vector3(
            planarVelocity.x,
            body.linearVelocity.y,
            planarVelocity.z
        );
    }
}
