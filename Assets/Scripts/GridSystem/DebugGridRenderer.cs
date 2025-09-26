using UnityEngine;

public class DebugGridRenderer : MonoBehaviour
{
    [Header("Settings")]
    public bool showGrid = true;
    public Color gridColor = Color.white;
    public float cellSize = 1f;
    public string gridPlaneTag = "GridPlane";
    
    private void OnDrawGizmos()
    {
        if (!showGrid) return;
        
        Gizmos.color = gridColor;
        
        // GridPlane tag'li tüm objeleri bul
        GameObject[] planes = GameObject.FindGameObjectsWithTag(gridPlaneTag);
        
        if (planes.Length == 0) return;
        
        foreach (GameObject plane in planes)
        {
            DrawGridForPlane(plane);
        }
    }
    
    private void DrawGridForPlane(GameObject plane)
    {
        Collider col = plane.GetComponent<Collider>();
        if (col == null) return;
        
        Bounds bounds = col.bounds;
        
        // Grid çizgileri
        float startX = bounds.min.x;
        float endX = bounds.max.x;
        float startZ = bounds.min.z;
        float endZ = bounds.max.z;
        float y = bounds.max.y + 0.01f;
        
        // Horizontal çizgiler
        for (float z = startZ; z <= endZ; z += cellSize)
        {
            Vector3 start = new Vector3(startX, y, z);
            Vector3 end = new Vector3(endX, y, z);
            Gizmos.DrawLine(start, end);
        }
        
        // Vertical çizgiler
        for (float x = startX; x <= endX; x += cellSize)
        {
            Vector3 start = new Vector3(x, y, startZ);
            Vector3 end = new Vector3(x, y, endZ);
            Gizmos.DrawLine(start, end);
        }
    }
}