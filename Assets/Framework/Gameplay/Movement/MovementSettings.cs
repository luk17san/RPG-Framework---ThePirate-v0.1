using UnityEngine;

[CreateAssetMenu(
    fileName = "MovementSettings",
    menuName = "Framework/Movement/Movement Settings"
)]
public class MovementSettings : ScriptableObject
{
    [Header("Throttle Speeds")]
    [Min(0f)] [SerializeField] private float reverseSpeed = 2.5f;
    [Min(0f)] [SerializeField] private float oarsSpeed = 4f;
    [Min(0f)] [SerializeField] private float partialSailsSpeed = 7f;
    [Min(0f)] [SerializeField] private float fullSailsSpeed = 10f;

    [Header("Speed Response")]
    [Min(0f)] [SerializeField] private float acceleration = 2f;
    [Min(0f)] [SerializeField] private float deceleration = 3.5f;

    [Header("Steering")]
    [Min(0f)] [SerializeField] private float turnSpeed = 45f;
    [Min(0f)] [SerializeField] private float steeringResponse = 4f;
    [Range(0f, 1f)] [SerializeField] private float turnAuthorityAtRest = 0.15f;

    public float ReverseSpeed => reverseSpeed;
    public float OarsSpeed => oarsSpeed;
    public float PartialSailsSpeed => partialSailsSpeed;
    public float FullSailsSpeed => fullSailsSpeed;
    public float Acceleration => acceleration;
    public float Deceleration => deceleration;
    public float TurnSpeed => turnSpeed;
    public float SteeringResponse => steeringResponse;
    public float TurnAuthorityAtRest => turnAuthorityAtRest;
}
