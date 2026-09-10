using UnityEngine;

public class HeroSwordAbility : BaseAbility
{
    protected override void OnAbilityStart()
    {
        player.IsAbilityLocked = true;
        player.IsMovementLocked = true;
        player.IsAttackLocked = true;
        player.StopMovement(); // Force a hard stop immediately

        // Play the beam animation (make sure this state exists in your Animator)
        player.Anim.Play("HeroSword_Beam", -1, 0f);
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
            HandleAoEDamage(collision, AttackType.Heavy);
        }
    }
}