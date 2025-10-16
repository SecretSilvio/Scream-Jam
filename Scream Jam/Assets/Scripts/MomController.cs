using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MomController : MonoBehaviour
{
    public Transform currentWaypoint;
    public Transform previousWaypoint;
    public List<Transform> waypoints = new List<Transform>();

    public float waypointThreshold = 1.0f;
    public float idleTime = 2.0f;

    private NavMeshAgent agent;
    [SerializeField]
    private State currentState = State.Idle;
    [SerializeField]
    private State previousState;
    private float idleTimer = 0.0f;

    public enum State
    {
        Idle,
        Walking,

    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject[] taggedWaypoints = GameObject.FindGameObjectsWithTag("MomWaypoint");
        foreach (GameObject obj in taggedWaypoints)
        {
            waypoints.Add(obj.transform);
        }

        if (currentWaypoint == null && waypoints.Count > 0)
        {
            currentWaypoint = waypoints[0];
        }

        ChangeState(State.Idle);

    }

    private void Update()
    {
        if (currentState != previousState)
        {
            OnEnterState(currentState);
            previousState = currentState;
        }

        switch (currentState)
        {
            case State.Idle:
                HandleIdleState();
                break;

            case State.Walking:
                HandleWalkingState();
                break;
        }
    }

    void OnEnterState(State state)
    {
        switch (state)
        {
            case State.Idle:
                EnterIdle();
                break;
            case State.Walking:
                EnterWalking();
                break;
        }
    }

    void EnterIdle()
    {
        agent.isStopped = true;
        idleTimer = 0.0f;
    }

    void EnterWalking()
    {
        agent.isStopped = false;
        //find neighbor of waypoint you are currently at that is NOT the waypoint before that (prevents doubling back)
        //set that neighbor as new current waypoint and set old current waypoint as previous waypoint
        if (currentWaypoint != null)
        {
            MomWaypoint waypointScript = currentWaypoint.GetComponent<MomWaypoint>();
            if (waypointScript != null && waypointScript.neighbors.Count > 0)
            {
                List<Transform> possibleNextWaypoints = new List<Transform>(waypointScript.neighbors);
                if (previousWaypoint != null)
                {
                    possibleNextWaypoints.Remove(previousWaypoint);
                }
                if (possibleNextWaypoints.Count > 0)
                {
                    Transform nextWaypoint = possibleNextWaypoints[Random.Range(0, possibleNextWaypoints.Count)];
                    previousWaypoint = currentWaypoint;
                    currentWaypoint = nextWaypoint;
                    MoveTo(currentWaypoint);

                    ChangeState(State.Walking);
                }
                else
                {
                    // No valid next waypoint found, stay idle
                    ChangeState(State.Idle);
                }
            }
            else
            {
                // No neighbors found, stay idle
                ChangeState(State.Idle);
            }
        }
        else
        {
            // No current waypoint, stay idle
            ChangeState(State.Idle);
        }
    }

    void MoveTo(Transform waypoint)
    {
        if (waypoint != null)
        {
            agent.SetDestination(waypoint.position);
        }
    }

    void HandleIdleState()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            ChangeState(State.Walking);
        }
    }

    void HandleWalkingState()
    {
        if (Vector3.Distance(transform.position, currentWaypoint.position) <= waypointThreshold)
        {
            ChangeState(State.Idle);
            return;
        }
    }

    void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

}
