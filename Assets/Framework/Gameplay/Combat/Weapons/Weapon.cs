using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField]
    private WeaponDefinition definition;

    [SerializeField]
    private WeaponSide side;

    [Header("Fire Point")]
    [SerializeField]
    private Transform firePoint;

    [Header("Projectile")]
    [SerializeField]
    private GameObject projectilePrefab;

    private float cooldownTimer;

    public WeaponDefinition Definition => definition;
    public WeaponSide Side => side;

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public bool CanFire()
    {
        return cooldownTimer <= 0f;
    }

    public void Fire()
    {
        if (!CanFire())
            return;

        if (definition == null)
        {
            Debug.LogWarning($"{name}: WeaponDefinition is missing.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning($"{name}: Fire Point is missing.");
            return;
        }

        GameObject prefab = projectilePrefab;

        if (prefab == null)
        {
            prefab = definition.projectilePrefab;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"{name}: Projectile prefab is missing.");
            return;
        }

        GameObject projectileObject = Instantiate(
            prefab,
            firePoint.position,
            firePoint.rotation
        );

        Projectile projectile =
            projectileObject.GetComponent<Projectile>();

        if (projectile == null)
        {
            Debug.LogWarning(
                $"{prefab.name}: Projectile component is missing."
            );

            Destroy(projectileObject);
            return;
        }

        projectile.Initialize(
            definition.damage,
            definition.projectileSpeed,
            definition.range
        );

        cooldownTimer = definition.cooldown;
    }
}