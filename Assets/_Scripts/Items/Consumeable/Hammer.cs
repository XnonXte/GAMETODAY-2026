using UnityEngine;

[CreateAssetMenu(menuName = "Consumeables/Hammer")]
public class Hammer : BaseItemSO
{
    [Header("Payload Fix Settings")]
    public float fixAmount = 80f;

    public override bool UseItem(Player player)
    {
        AudioManager.Instance.PlayAudio(AudioManager.Instance.SFX_Hammer);
        EventHandler.WhenHammerAbilityUsed(fixAmount);
        return true;
    }

}
