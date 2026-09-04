using UnityEngine;

public class BroadsideCooldown : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField]
    private float cooldownDuration = 4f;

    private float cooldownTimer;

    public bool IsReady => cooldownTimer <= 0f;

    public float RemainingTime => cooldownTimer;

    public float NormalizedRemainingTime
    {
        get
        {
            if (cooldownDuration <= 0f)
                return 0f;

            return Mathf.Clamp01(
                cooldownTimer / cooldownDuration
            );
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0f)
            {
                cooldownTimer = 0f;
            }
        }
    }

    public bool TryStartCooldown()
    {
        if (!IsReady)
            return false;

        cooldownTimer = cooldownDuration;

        return true;
    }

    public void ResetCooldown()
    {
        cooldownTimer = 0f;
    }
}