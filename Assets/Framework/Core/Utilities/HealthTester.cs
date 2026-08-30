using UnityEngine;
using UnityEngine.InputSystem;

public class HealthTester : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError(
                $"HealthTester na obiekcie '{gameObject.name}' nie znalaz³ komponentu Health!",
                gameObject
            );

            enabled = false;
            return;
        }

        health.HealthChanged += OnHealthChanged;
        health.Died += OnDied;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            health.TakeDamage(10f);
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            health.Heal(10f);
        }
    }

    private void OnHealthChanged(float currentHealth)
    {
        Debug.Log(
            $"Health: {currentHealth}/{health.MaxHealth}"
        );
    }

    private void OnDied()
    {
        Debug.Log("Object died.");
    }

    private void OnDestroy()
    {
        if (health == null)
            return;

        health.HealthChanged -= OnHealthChanged;
        health.Died -= OnDied;
    }
}