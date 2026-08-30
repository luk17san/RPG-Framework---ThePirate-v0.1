using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }

    public float MaxHealth => maxHealth;

    public bool IsDead => CurrentHealth <= 0f;

    public event Action<float> HealthChanged;

    public event Action Died;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead)
            return;

        if (amount <= 0f)
            return;

        CurrentHealth -= amount;

        CurrentHealth = Mathf.Clamp(
            CurrentHealth,
            0f,
            MaxHealth
        );

        HealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0f)
        {
            Died?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead)
            return;

        if (amount <= 0f)
            return;

        CurrentHealth += amount;

        CurrentHealth = Mathf.Clamp(
            CurrentHealth,
            0f,
            MaxHealth
        );

        HealthChanged?.Invoke(CurrentHealth);
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = Mathf.Max(1f, value);

        CurrentHealth = Mathf.Clamp(
            CurrentHealth,
            0f,
            maxHealth
        );

        HealthChanged?.Invoke(CurrentHealth);
    }
}