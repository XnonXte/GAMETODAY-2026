using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class WeaponItemSO : BaseItemSO
{
    [Header("Ability Settings")]
    public GameObject abilityPrefab;

    public override bool UseItem(Player player)
    {
        if (abilityPrefab != null)
        {
            Instantiate(abilityPrefab, player.transform.position + Vector3.up * 1.5f, Quaternion.identity, player.transform);
            return true; 
        }

        Debug.LogWarning("No ability prefab assigned to this weapon!");
        return false;
    }
}
