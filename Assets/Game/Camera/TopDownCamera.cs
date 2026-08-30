using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("World-Space Camera")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 20f, -10f);
    [SerializeField] private Vector3 fixedEulerAngles = new Vector3(55f, 0f, 0f);

    [Header("Follow")]
    [Min(0f)] [SerializeField] private float followSharpness = 6f;

    private Quaternion fixedRotation;

    private void Awake()
    {
        fixedRotation = Quaternion.Euler(fixedEulerAngles);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        float interpolation = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            interpolation
        );

        transform.rotation = fixedRotation;
    }
}
