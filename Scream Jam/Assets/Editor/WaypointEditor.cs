using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointManager))]
public class WaypointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Find Neighbors"))
        {
            WaypointManager wp = (WaypointManager)target;
            wp.SetupWaypoints();
            EditorUtility.SetDirty(wp);
        }

        if (GUILayout.Button("Update Waypoint List"))
        {
            WaypointManager wp = (WaypointManager)target;
            wp.waypoints.Clear();
            wp.waypoints.AddRange(FindObjectsOfType<Waypoint>());
            EditorUtility.SetDirty(wp);
        }
    }
}