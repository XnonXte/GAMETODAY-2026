using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [SerializeField] private Vector2[] slotOffsets = { new Vector2(1.5f, 0), new Vector2(-1.5f, 0), new Vector2(0, 1f), new Vector2(0, -1f) };
    [SerializeField] private Vector2[] payloadSlotOffsets = { new Vector2(3f, 0), new Vector2(-3f, 0), new Vector2(0, 2f), new Vector2(0, -2f) };
    private Dictionary<Enemy, int> occupiedSlots = new Dictionary<Enemy, int>();

    private void Awake() => Instance = this;

    // 1. Add 'Transform target' as a parameter
    public bool RequestSlot(Enemy enemy, Transform target, out Vector3 slotPosition)
    {
        slotPosition = Vector3.zero;
        if (target == null) return false;

        Vector2[] activeOffsets = target.CompareTag("Payload") ? payloadSlotOffsets : slotOffsets;

        if (occupiedSlots.ContainsKey(enemy))
        {
            // 2. Replace transform.position with target.position
            slotPosition = target.position + (Vector3)activeOffsets[occupiedSlots[enemy]];
            return true;
        }

        for (int i = 0; i < slotOffsets.Length; i++)
        {
            if (!occupiedSlots.ContainsValue(i))
            {
                occupiedSlots.Add(enemy, i);
                // 3. Replace transform.position with target.position
                slotPosition = target.position + (Vector3)activeOffsets[i];
                return true;
            }
        }
        return false;
    }

    public void ReleaseSlot(Enemy enemy)
    {
        if (occupiedSlots.ContainsKey(enemy)) occupiedSlots.Remove(enemy);
    }
}