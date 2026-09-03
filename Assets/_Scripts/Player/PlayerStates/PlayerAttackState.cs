using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int currentAttack;
    private bool canCombo;
    private bool queuedAttack;

    private const int MAX_COMBO = 3;

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine)
        : base(player, playerStateMachine)
    {
    }

    public override void Enter()
    {
        currentAttack = 1;
        canCombo = false;
        queuedAttack = false;

        PlayAttack();
    }

    public override void Update()
    {
        player.StopMovement();

        // Player can only queue the next attack
        // during the combo window.
        if (canCombo && InputManager.instance.GetPlayerAttack())
        {
            queuedAttack = true;
            canCombo = false;

            Debug.Log("Next attack queued!");
        }
    }

    public override void Exit()
    {
        player.StopMovement();

        canCombo = false;
        queuedAttack = false;
    }

    private void PlayAttack()
    {
        Debug.Log("Playing Attack " + currentAttack);

        canCombo = false;
        queuedAttack = false;

        switch (currentAttack)
        {
            case 1:
                player.playerAnimator.SetTrigger("Attack1");
                break;

            case 2:
                player.playerAnimator.SetTrigger("Attack2");
                break;

            case 3:
                player.playerAnimator.SetTrigger("Attack3");
                break;
        }
    }

    // ==========================================
    // ANIMATION EVENTS
    // ==========================================

    public void AnimationAttackHit()
    {
        Debug.Log("Attack " + currentAttack + " HIT!");

        if (currentAttack == 3)
        {
            player.playerCombat.HeavyAttack();
        }
        else
        {
            player.playerCombat.Attack();
        }
    }

    public void OpenComboWindow()
    {
        Debug.Log("Combo window OPEN");

        canCombo = true;
    }

    public void CloseComboWindow()
    {
        Debug.Log("Combo window CLOSED");

        canCombo = false;
    }

    public void AttackFinished()
    {
        Debug.Log("Attack " + currentAttack + " FINISHED");

        canCombo = false;

        if (queuedAttack && currentAttack < MAX_COMBO)
        {
            currentAttack++;

            PlayAttack();
            return;
        }

        Debug.Log("Combo finished -> Idle");

        playerStateMachine.ChangeState(player.idleState);
    }

    public void ComboFinished()
    {
        Debug.Log("Attack 3 finished -> Idle");

        canCombo = false;
        playerStateMachine.ChangeState(player.idleState);
    }
}
