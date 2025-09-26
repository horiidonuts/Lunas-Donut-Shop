using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 1f;
    
    [Header("Visualization")]
    [SerializeField] private bool showGrid = true;
    [SerializeField] private bool showCellCenters = false;
    [SerializeField] private Color gridColor = Color.white;
    [SerializeField] private Color cellCenterColor = Color.red;
    
    private GridCell[,] gridArray;
    
    private void Start()
    {
        CreateGrid();
    }
    
    private void CreateGrid()
    {
        gridArray = new GridCell[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                gridArray[x, y] = new GridCell(x, y, worldPosition);
            }
        }
        
        Debug.Log($"Grid created: {width}x{height} cells at position {transform.position}");
        Debug.Log($"Cell size: {cellSize}, Total size: {width * cellSize}x{height * cellSize}");
    }
    
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + transform.position;
    }
    
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        Vector3 localPosition = worldPosition - transform.position;
        x = Mathf.FloorToInt(localPosition.x / cellSize);
        y = Mathf.FloorToInt(localPosition.z / cellSize);
    }
    
    public GridCell GetGridCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return null;
    }
    
    public GridCell GetGridCell(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetGridCell(x, y);
    }
    
    private void OnDrawGizmos()
    {
        // Her zaman grid çiz (seçili olmasa da)
        DrawGrid();
    }
    
    private void OnDrawGizmosSelected()
    {
        // Seçildiğinde de çiz
        DrawGrid();
    }
    
    private void DrawGrid()
    {
        if (!showGrid) return;
        
        Gizmos.color = gridColor;
        
        Vector3 pos = transform.position;
        
        // Horizontal lines
        for (int y = 0; y <= height; y++)
        {
            Vector3 start = pos + new Vector3(0, 0, y * cellSize);
            Vector3 end = pos + new Vector3(width * cellSize, 0, y * cellSize);
            Gizmos.DrawLine(start, end);
        }
        
        // Vertical lines
        for (int x = 0; x <= width; x++)
        {
            Vector3 start = pos + new Vector3(x * cellSize, 0, 0);
            Vector3 end = pos + new Vector3(x * cellSize, 0, height * cellSize);
            Gizmos.DrawLine(start, end);
        }
        
        // Draw cell centers (optional)
        if (showCellCenters && Application.isPlaying && gridArray != null)
        {
            Gizmos.color = cellCenterColor;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 cellCenter = GetWorldPosition(x, y) + new Vector3(cellSize * 0.5f, 0.1f, cellSize * 0.5f);
                    Gizmos.DrawWireCube(cellCenter, Vector3.one * 0.2f);
                }
            }
        }
        
        // Grid köşelerini işaretle
        Gizmos.color = Color.green;
        Vector3 corner1 = pos;
        Vector3 corner2 = pos + new Vector3(width * cellSize, 0, 0);
        Vector3 corner3 = pos + new Vector3(0, 0, height * cellSize);
        Vector3 corner4 = pos + new Vector3(width * cellSize, 0, height * cellSize);
        
        Gizmos.DrawWireCube(corner1, Vector3.one * 0.3f);
        Gizmos.DrawWireCube(corner2, Vector3.one * 0.3f);
        Gizmos.DrawWireCube(corner3, Vector3.one * 0.3f);
        Gizmos.DrawWireCube(corner4, Vector3.one * 0.3f);
    }
}
