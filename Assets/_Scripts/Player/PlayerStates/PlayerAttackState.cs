using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int comboStep = 0;
    private float failSafeTimer;

    //input buffer
    private float inputBufferTimer = 0f;
    private float bufferWindowDuration = 0.25f;

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.StopMovement();

        comboStep = 0;
        inputBufferTimer = 0f;

        PlayComboAnimation();
    }

    public override void Update()
    {
        base.Update();

        if (InputManager.Instance != null && InputManager.Instance.GetPlayerAttack())
        {
            inputBufferTimer = bufferWindowDuration;
        }

        if (inputBufferTimer > 0f)
        {
            inputBufferTimer -= Time.deltaTime;
        }

        failSafeTimer -= Time.deltaTime;
        if (failSafeTimer <= 0f)
        {
            ForceExitState();
        }
    }

    public void TriggerAttackHitbox()
    {
        AttackType currentAttackType = (comboStep == 2) ? AttackType.Heavy : AttackType.Light;
        int facingDirection = (int)Mathf.Sign(player.transform.localScale.x);

        if (player.Combat != null) player.Combat.Attack(currentAttackType, facingDirection);
    }

    public void AnimationFinishTrigger()
    {
        if (inputBufferTimer > 0f && comboStep < 2)
        {
            comboStep++;
            inputBufferTimer = 0f; 
            PlayComboAnimation();
        }
        else
        {
            ForceExitState();
        }
    }

    private void PlayComboAnimation()
    {
        string animName = "PlayerAttack" + comboStep;
        player.Anim.Play(animName, -1, 0f);
        failSafeTimer = 1.0f;
    }

    private void ForceExitState()
    {
        if (InputManager.Instance != null && InputManager.Instance.GetPlayerMovement().sqrMagnitude > 0.01f)
        {
            playerStateMachine.ChangeState(player.WalkState);
        }
        else
        {
            playerStateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StopMovement();
    }
}