using UnityEngine;

public class PlayerAttackState : PlayerState
{
    protected override string AnimBoolName => "isIdling";
    private int comboStep = 0;
    private float failSafeTimer;

    // Input buffer
    private bool attackBuffered = false;
    private float inputBufferTimer = 0f;
    [SerializeField] private float bufferWindowDuration = 0.6f; // Shortened to a standard responsive window

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.StopMovement();

        comboStep = 0;
        attackBuffered = false;
        inputBufferTimer = 0f;

        PlayComboAnimation();
    }

    public override void Update()
    {
        base.Update();

        // Capture input the moment the player presses attack during the combo animation
        if (InputManager.Instance != null && InputManager.Instance.GetPlayerAttack())
        {
            attackBuffered = true;
            inputBufferTimer = bufferWindowDuration;
        }

        if (attackBuffered)
        {
            inputBufferTimer -= Time.deltaTime;
            if (inputBufferTimer <= 0f)
            {
                attackBuffered = false;
            }
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
        // Check if an attack was explicitly buffered during the animation and we haven't reached the final step
        if (attackBuffered && comboStep < 2)
        {
            comboStep++;
            attackBuffered = false;
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
        player.SwordAnim.Play(animName, -1, 0f);
        failSafeTimer = 1.5f; // Give enough safety room for the animation length
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

        if (player.SwordAnim != null)
        {
            player.SwordAnim.Play("SwordNormal", -1, 0f);
        }
    }
}