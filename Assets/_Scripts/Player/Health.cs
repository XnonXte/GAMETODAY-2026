using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHp;
    private float currentHp;

    public UnityEvent<float> Healed;
    public UnityEvent<float> Damaged;
    public UnityEvent Died;

    public float CurrentHP
    {
        get => currentHp;

        private set
        {
            var isDamage = value < 0;
            currentHp = Mathf.Clamp(value, 0, maxHp);

            if(isDamage) Damaged?.Invoke(currentHp);
            else Healed?.Invoke(currentHp);

            if (currentHp <= 0) Died?.Invoke();
        }
    }

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void Damage(float amount) => CurrentHP -= amount;
    public void Heal(float amount) => CurrentHP += amount;
    public void HealFull() => CurrentHP = maxHp;
    public void Kill() => CurrentHP = 0;
    public void Adjust(int value) => CurrentHP = value;
}
