using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    public event Action<Vector2, AttackType> OnDamaged;
    public event Action OnDeath;
    [SerializeField] private float maxHp = 100;
    private float currentHp;
    public float CurrentHp => currentHp;
    private bool hasLoadedSavedHealth = false;
    private void Start()
    {
        if (!hasLoadedSavedHealth)
        {
            currentHp = maxHp;
        }

        if (GetComponent<Player>() != null) EventHandler.WhenPlayerHealthChanged(currentHp, maxHp);
        if (GetComponent<Payload>() != null) EventHandler.WhenPayloadHealthChanged(currentHp, maxHp);
    }

    public void TakeDamage(float amount, Vector2 direction, AttackType attackType)
    {
        ChangeHealth(amount * -1, direction, attackType);
    }

    public void ChangeHealth(float amount, Vector2 direction, AttackType attackType)
    {
        currentHp += amount;
        if (currentHp > maxHp) { currentHp = maxHp; }
        else if (currentHp <= 0) { OnDeath?.Invoke(); }
        else if (amount < 0) 
        {
            AudioManager.Instance.PlayAudio(AudioManager.Instance.SFX_Hit);
            OnDamaged?.Invoke(direction, attackType); 
        }

        if (GetComponent<Player>() != null) EventHandler.WhenPlayerHealthChanged(currentHp, maxHp);
        if (GetComponent<Payload>() != null) EventHandler.WhenPayloadHealthChanged(currentHp, maxHp);

        Debug.Log($"{gameObject.name} Current HP = {currentHp}");
    }

    public void LoadSavedHealth(float savedAmount)
    {
        currentHp = savedAmount;
        hasLoadedSavedHealth = true;

        // Update the UI immediately so it doesn't briefly show 100% health
        if (GetComponent<Player>() != null) EventHandler.WhenPlayerHealthChanged(currentHp, maxHp);
        if (GetComponent<Payload>() != null) EventHandler.WhenPayloadHealthChanged(currentHp, maxHp);
    }
}
