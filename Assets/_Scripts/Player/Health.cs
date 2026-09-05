using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    public event Action OnDamaged;
    public event Action OnDeath;

    [SerializeField] private float maxHp = 100;
    private float currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float amount, Vector2 direction)
    {
        ChangeHealth(amount);
    }

    public void ChangeHealth(float amount)
    {
        currentHp += amount;
        if (currentHp > maxHp) { currentHp = maxHp; }
        else if (currentHp <= 0) { OnDeath?.Invoke(); }
        else if (amount < 0) { OnDamaged?.Invoke(); }
    }
}
