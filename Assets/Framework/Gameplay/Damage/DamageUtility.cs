using UnityEngine;

public static class DamageUtility
{
    public static bool TryDealDamage(
        GameObject target,
        float amount)
    {
        if (target == null)
            return false;

        if (!target.TryGetComponent<IDamageable>(
                out var damageable))
        {
            return false;
        }

        damageable.TakeDamage(amount);

        return true;
    }
}