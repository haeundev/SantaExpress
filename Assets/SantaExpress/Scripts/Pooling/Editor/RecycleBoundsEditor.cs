using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RecycleBounds))]
public class RecycleBoundsEditor : Editor
{
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    private static void DrawGameObjectName(RecycleBounds bounds, GizmoType gizmoType)
    {
        var style = new GUIStyle();
        Vector3 v3FrontTopLeft;

        if (bounds.boundary.size != Vector3.zero)
        {
            style.normal.textColor = Color.magenta;
            v3FrontTopLeft = new Vector3(bounds.boundary.center.x - bounds.boundary.extents.x,
                bounds.boundary.center.y + bounds.boundary.extents.y + 1,
                bounds.boundary.center.z - bounds.boundary.extents.z); // Front top left corner
            Handles.Label(v3FrontTopLeft, "Recycle Bounds", style);
            VisualizedDebug.DrawHandlesBounds(bounds.boundary, Color.magenta);
        }
    }
}