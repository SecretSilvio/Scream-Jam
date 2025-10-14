using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float neighborRadius = 5f;
    [SerializeField]
    public List<Waypoint> neighbors = new List<Waypoint>();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, neighborRadius);

        Gizmos.color = Color.green;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }

    // Call this to auto-populate neighbors
    public void FindNeighbors()
    {
        neighbors.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, neighborRadius);
        foreach (var hit in hits)
        {
            if (hit.transform == this.transform) continue;
            Waypoint wp = hit.GetComponent<Waypoint>();
            if (wp != null)
                neighbors.Add(wp);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
