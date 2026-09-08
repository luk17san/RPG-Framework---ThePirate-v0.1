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
        float gearLimit = stats.GetTargetSpeed(throttle.CurrentLevel);

        float speedInput = Mathf.Clamp(
            movementInput.SpeedInput,
            -1f,
            1f
        );

        bool isReverseGear =
            throttle.CurrentLevel == MovementThrottleLevel.Reverse;

        // Domyœlnie utrzymujemy aktualn¹ prêdkoœæ.
        float targetSpeed = currentForwardSpeed;

        // W/S steruj¹ przyspieszaniem i hamowaniem.
        if (speedInput != 0f)
        {
            bool isAccelerating = speedInput > 0f;

            // Na biegu wstecznym S przyspiesza,
            // a W hamuje cofanie.
            if (isReverseGear)
                isAccelerating = speedInput < 0f;

            targetSpeed = isAccelerating
                ? gearLimit
                : 0f;
        }

        // Nie pozwalamy przekroczyæ limitu wybranego biegu.
        targetSpeed = ClampSpeedToGear(
            targetSpeed,
            gearLimit
        );

        // Dobieramy przyspieszanie lub hamowanie.
        float rate = GetSpeedChangeRate(targetSpeed);

        currentForwardSpeed = Mathf.MoveTowards(
            currentForwardSpeed,
            targetSpeed,
            rate * Time.fixedDeltaTime
        );
    }

    private float ClampSpeedToGear(float speed, float gearLimit)
    {
        if (gearLimit > 0f)
            return Mathf.Clamp(speed, 0f, gearLimit);

        if (gearLimit < 0f)
            return Mathf.Clamp(speed, gearLimit, 0f);

        return 0f;
    }

    private float GetSpeedChangeRate(float targetSpeed)
    {
        bool changesDirection =
            currentForwardSpeed * targetSpeed < 0f;

        bool increasesSpeed =
            Mathf.Abs(targetSpeed) > Mathf.Abs(currentForwardSpeed) &&
            !changesDirection;

        return increasesSpeed
            ? stats.GetAcceleration(throttle.CurrentLevel)
            : stats.GetDeceleration(throttle.CurrentLevel);
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

        float turnMultiplier =
    stats.GetTurnMultiplier(throttle.CurrentLevel);

        float turnAmount =
            currentTurn *
            stats.TurnSpeed *
            turnAuthority *
            turnMultiplier *
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
