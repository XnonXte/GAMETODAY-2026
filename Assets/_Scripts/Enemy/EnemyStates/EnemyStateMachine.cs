using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState CurrentEnemyState { get; private set; }
    public void Initialize(EnemyState startingState)
    {
        CurrentEnemyState = startingState;
        CurrentEnemyState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentEnemyState.Exit();
        CurrentEnemyState = newState;
        CurrentEnemyState.Enter();
        Debug.Log($"Current State: {CurrentEnemyState}");
    }
}
