using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AttackHit()
    {
        player.playerAttackAnimationHit();
    }

    public void OpenComboWindow()
    {
        player.playerAttackOpenComboWindow();
    }

    public void AttackFinished()
    {
        player.playerAttackFinished();
    }

    public void ComboFinished()
    {
        player.playerAttackComboFinished();
    }
}
