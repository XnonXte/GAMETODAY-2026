using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentPlayerState { get; private set; }

    public void Initialize(PlayerState startingState)
    {
        currentPlayerState = startingState;
        currentPlayerState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        currentPlayerState.Exit();
        currentPlayerState = newState;
        currentPlayerState.Enter();
        Debug.Log($"Current State: {currentPlayerState}");
    }
}

