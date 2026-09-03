using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        StateMachine = new();
    }

    private void Update() => StateMachine.CurrentEnemyState?.Update();
    private void FixedUpdate() => StateMachine.CurrentEnemyState?.FixedUpdate();
}
