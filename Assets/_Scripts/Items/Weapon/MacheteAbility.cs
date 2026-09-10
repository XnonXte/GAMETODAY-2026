using UnityEngine;

public class MacheteAbility : BaseAbility
{
    [Header("Machete Specifics")]
    [SerializeField] private float speedBoostAmount = 4f;
    private Animator anim;

    protected override void OnAbilityStart()
    {
        // 1. Buff the player's speed using your Player.cs method
        player.ApplySpeedBoost(speedBoostAmount, duration);

        // 2. Play the frenzy aura visual effect
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("MacheteFrenzy_Anim");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HandleAoEDamage(collision, AttackType.Light);
        }
    }
}