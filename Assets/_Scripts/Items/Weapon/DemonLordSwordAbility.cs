using UnityEngine;

public class DemonLordSwordAbility : BaseAbility
{
    [Header("Demon Lord Specifics")]
    [SerializeField] private GameObject flyingSlashProjectilePrefab; 
    [SerializeField] private float timeBetweenSlashes = 1f;
    private float nextSlashTime;

    protected override void OnAbilityStart()
    {
        player.IsAttackLocked = true;
    }

    private void Update()
    {
        if (Time.time >= nextSlashTime)
        {
            nextSlashTime = Time.time + timeBetweenSlashes;

            //player.Anim.Play("DemonLord_Swing", -1, 0f);

            // 1. Spawn the projectile
            GameObject slash = Instantiate(flyingSlashProjectilePrefab, player.transform.position + Vector3.up * 1.5f, Quaternion.identity);

            // 2. Pass the player's facing direction and damage multiplier!
            FlyingSlashProjectile projectileScript = slash.GetComponent<FlyingSlashProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(player.transform.localScale.x, player.Combat.DamageMultiplier);
            }
        }
    }

    protected override void OnAbilityEnd()
    {
        player.IsAttackLocked = false;
    }
}