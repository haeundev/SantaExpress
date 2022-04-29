using SantaExpress.Scripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EndlessLandGenerator))]
public class EndlessLandGeneratorEditor : Editor
{
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    private static void DrawGameObjectName(EndlessLandGenerator land, GizmoType gizmoType)
    {
        var bounds = land.areaBounds;
        var style = new GUIStyle();
        Vector3 v3FrontTopLeft;

        if (bounds.size != Vector3.zero)
        {
            style.normal.textColor = Color.magenta;
            v3FrontTopLeft = new Vector3(bounds.center.x - bounds.extents.x,
                bounds.center.y + bounds.extents.y + 1,
                bounds.center.z - bounds.extents.z); // Front top left corner
            Handles.Label(v3FrontTopLeft, "Recycle Bounds", style);
            VisualizedDebug.DrawHandlesBounds(bounds, Color.magenta);
        }
    }
}