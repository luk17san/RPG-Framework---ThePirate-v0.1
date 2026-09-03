using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float speed;
    private float maxDistance;

    private Vector3 startPosition;

    public void Initialize(
        float damage,
        float speed,
        float range)
    {
        this.damage = damage;
        this.speed = speed;
        this.maxDistance = range;

        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position +=
            transform.forward * speed * Time.deltaTime;

        float distance = Vector3.Distance(
            startPosition,
            transform.position
        );

        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hit = DamageUtility.TryDealDamage(
            other.gameObject,
            damage
        );

        if (hit)
        {
            Destroy(gameObject);
        }
    }
}