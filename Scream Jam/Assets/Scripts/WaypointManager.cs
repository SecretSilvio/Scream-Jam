using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField]
    public List<Waypoint> waypoints = new List<Waypoint>();

    public void SetupWaypoints()
    {
        foreach (var wp in waypoints)
        {
            wp.FindNeighbors();
        }
    }
}
