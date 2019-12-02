using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(HeatMap))]
public class HeatMapEditor : Editor
{
    private bool current;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // HeatMap class values
        HeatMap myTarget = (HeatMap)target;

        EditorGUILayout.Separator(); // Button Controls
        if (GUILayout.Button(myTarget.GridCreated() ? "Reload" : "Build"))
            myTarget.ReloadGrid();
        if (GUILayout.Button("Clear"))
            myTarget.ClearGrid();

        EditorGUILayout.Separator(); // Type & Scale
        HeatMap.HeatmapSHOW current_type = myTarget.type;
        myTarget.SetType((HeatMap.HeatmapSHOW)EditorGUILayout.EnumPopup("Type", current_type));
        myTarget.UpdateYScale(EditorGUILayout.FloatField("Y Scale", myTarget.y_scale));

        EditorGUILayout.Separator(); // Normalize
        myTarget.ToggleNormalized(GUILayout.Toggle(current = myTarget.normalized, "Normalize"));

        EditorGUILayout.Separator(); // Hide Empty
        myTarget.ToggleHideEmpty(GUILayout.Toggle(current = myTarget.hide_empty, "Hide Empty"));

        EditorGUILayout.Separator(); // Data info
        GUILayout.Label("Total Entries: " + myTarget.entries_count);
        GUILayout.Label("Max Speed: " + myTarget.max_speed);
        GUILayout.Label("Max Entries per cell: " + myTarget.max_entries_per_grid);
        GUILayout.Label("Max Average Speed per cell: " + myTarget.max_average_speed);
    }
}
