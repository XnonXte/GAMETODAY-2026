using UnityEngine;

public class AxeAbility : BaseAbility
{
    protected override void OnAbilityStart()
    {
        // Grab the Animator on THIS prefab (not the player's animator)
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("AxeSpin_Anim");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HandleAoEDamage(collision, AttackType.Heavy);
        }
    }
}