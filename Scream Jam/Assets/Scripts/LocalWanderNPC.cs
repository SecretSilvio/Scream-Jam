using UnityEngine;
using UnityEngine.AI;

public class LocalWanderNPC : MonoBehaviour
{
    public Waypoint currentWaypoint;
    private NavMeshAgent agent;

    private float waitTime = 1f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (currentWaypoint == null)
        {
            Debug.LogError("No starting waypoint assigned.");
            enabled = false;
            return;
        }

        GoToNextWaypoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                waitTimer = 0f;
                isWaiting = true;
            }

            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                GoToNextWaypoint();
                isWaiting = false;
            }
        }
    }

    void GoToNextWaypoint()
    {
        if (currentWaypoint.neighbors.Count == 0)
        {
            Debug.LogWarning("No neighbors to move to.");
            return;
        }

        // Pick a random neighbor
        Waypoint next = currentWaypoint.neighbors[Random.Range(0, currentWaypoint.neighbors.Count)];
        currentWaypoint = next;
        agent.SetDestination(next.transform.position);
    }
}
