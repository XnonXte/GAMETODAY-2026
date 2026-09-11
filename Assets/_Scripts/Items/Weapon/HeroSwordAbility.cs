using UnityEngine;

public class HeroSwordAbility : BaseAbility
{
    private Animator anim;
    protected override void OnAbilityStart()
    {
        player.IsAbilityLocked = true;
        player.IsMovementLocked = true;
        player.IsAttackLocked = true;
        player.StopMovement(); // Force a hard stop immediately

        anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.Play("HeroSword_Beam");
        }
    }

    protected override void OnAbilityEnd()
    {
        player.IsAbilityLocked = false;
        player.IsMovementLocked = false;
        player.IsAttackLocked = false;

        player.StateMachine.ChangeState(player.IdleState);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HandleAoEDamage(collision, AttackType.Light);
        }
    }
}