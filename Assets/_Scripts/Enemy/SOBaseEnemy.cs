using UnityEngine;

public class SOBaseEnemy : ScriptableObject
{
    //[Header("Enemy Base Attributes")]
    [field: SerializeField] public float maxHealth { get; private set; }
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public float attackDamage { get; private set; }

    //[Header("Enemy Visual")]
    [field: SerializeField] public Sprite enemySprite { get; private set; }
}
