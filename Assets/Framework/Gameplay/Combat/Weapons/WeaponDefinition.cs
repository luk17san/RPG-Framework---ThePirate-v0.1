using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponDefinition",
    menuName = "Game/Combat/Weapon Definition"
)]
public class WeaponDefinition : ScriptableObject
{
    [Header("Basic")]
    public string weaponName;
    public WeaponType weaponType;

    [Header("Damage")]
    public float damage = 10f;

    [Header("Combat")]
    public float range = 20f;
    public float cooldown = 1f;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
}