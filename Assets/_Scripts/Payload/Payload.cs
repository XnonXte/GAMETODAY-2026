using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Rigidbody2D))]
public class Payload : MonoBehaviour
{
    public static Payload Instance { get; private set; }

    [Header("Movement & Pathing")]
    [SerializeField] private List<Transform> pathWaypoints; // Drag all your arena stops here in order!
    [SerializeField] private float moveSpeed = 3f;

    private Transform currentDestination;
    private int waypointIndex = 0;
    private Health healthComponent;
    private bool hasReachedDestination = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        healthComponent = GetComponent<Health>();
    }

    private void Start()
    {
        // Automatically set the first destination on start if we have waypoints
        if (pathWaypoints != null && pathWaypoints.Count > 0)
        {
            SetNextDestination();
        }
    }

    private void OnEnable()
    {
        healthComponent.OnDamaged += HandleDamageTaken;
        healthComponent.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        healthComponent.OnDamaged -= HandleDamageTaken;
        healthComponent.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        if (hasReachedDestination || currentDestination == null) return;

        // Move towards the current destination
        transform.position = Vector2.MoveTowards(transform.position, currentDestination.position, moveSpeed * Time.deltaTime);

        // Check if it reached the destination
        if (Vector2.Distance(transform.position, currentDestination.position) < 0.1f)
        {
            hasReachedDestination = true;
            Debug.Log("Payload has reached the arena zone and stopped.");
        }
    }

    // Call this method whenever a wave ends to advance to the next point in your list
    public void SetNextDestination()
    {
        if (pathWaypoints == null || waypointIndex >= pathWaypoints.Count)
        {
            Debug.Log("Payload has reached the final destination!");
            return;
        }

        currentDestination = pathWaypoints[waypointIndex];
        waypointIndex++;
        hasReachedDestination = false; // Unlocks movement in Update()
        Debug.Log($"Payload moving to waypoint {waypointIndex}");
    }

    private void HandleDamageTaken(Vector2 direction, AttackType attackType)
    {
        Debug.Log($"[Payload] Warning! The payload took damage from a {attackType} attack!");
    }

    private void HandleDeath()
    {
        Debug.Log("Payload destroyed! Game Over.");
        Destroy(gameObject);
    }
}