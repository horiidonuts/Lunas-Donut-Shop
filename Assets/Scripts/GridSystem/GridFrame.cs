using UnityEngine;
using System.Collections.Generic;

public class GridFrame : MonoBehaviour
{
    [Header("Frame Settings")]
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector3 frameOffset = Vector3.zero;
    
    [Header("Visual Settings")]
    [SerializeField] private bool showFrame = true;
    [SerializeField] private Color frameColor = Color.yellow;
    [SerializeField] private float frameLineWidth = 0.05f;
    
    [Header("Runtime Grid Lines")]
    [SerializeField] private bool showRuntimeGridLines = true;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Color runtimeGridLineColor = Color.white;
    [SerializeField] private float runtimeLineWidth = 0.02f;
    [SerializeField] private float lineHeight = 0.01f; // Y ekseninde hafif yukarıda olsun
    
    [Header("Gizmo Grid Lines (Editor Only)")]
    [SerializeField] private bool showGizmoGridLines = false;
    [SerializeField] private Color gizmoGridLineColor = Color.white;
    [SerializeField] private float gizmoGridLineAlpha = 0.5f;
    
    [Header("Corner Markers")]
    [SerializeField] private bool showCorners = true;
    [SerializeField] private Color cornerColor = Color.green;
    [SerializeField] private float cornerSize = 0.2f;
    
    private List<GameObject> runtimeLines = new List<GameObject>();
    
    void Start()
    {
        if (showRuntimeGridLines)
        {
            CreateRuntimeGridLines();
        }
    }
    
    void OnDestroy()
    {
        DestroyRuntimeGridLines();
    }
    
    void OnValidate()
    {
        // Editor'da değişiklik yapıldığında runtime çizgilerini güncelle
        if (Application.isPlaying && showRuntimeGridLines)
        {
            DestroyRuntimeGridLines();
            CreateRuntimeGridLines();
        }
    }
    
    void OnDrawGizmos()
    {
        if (showFrame)
        {
            DrawFrame();
        }
        
        if (showGizmoGridLines)
        {
            DrawGizmoGridLines();
        }
        
        if (showCorners)
        {
            DrawCorners();
        }
    }
    
    void CreateRuntimeGridLines()
    {
        DestroyRuntimeGridLines(); // Önce eskilerini temizle
        
        Vector3 origin = transform.position + frameOffset;
        float width = gridSize.x * cellSize;
        float height = gridSize.y * cellSize;
        
        // Dikey çizgiler
        for (int x = 1; x < gridSize.x; x++)
        {
            Vector3 start = origin + new Vector3(x * cellSize, lineHeight, 0);
            Vector3 end = start + new Vector3(0, 0, height);
            CreateLine($"VerticalLine_{x}", start, end);
        }
        
        // Yatay çizgiler
        for (int z = 1; z < gridSize.y; z++)
        {
            Vector3 start = origin + new Vector3(0, lineHeight, z * cellSize);
            Vector3 end = start + new Vector3(width, 0, 0);
            CreateLine($"HorizontalLine_{z}", start, end);
        }
    }
    
    void CreateLine(string name, Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject(name);
        lineObj.transform.SetParent(transform);
        
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        
        // Materyal ayarla
        if (lineMaterial != null)
        {
            lr.material = lineMaterial;
        }
        else
        {
            // Varsayılan materyal oluştur
            lr.material = CreateDefaultLineMaterial();
        }
        
        lr.startColor = runtimeGridLineColor;
        lr.endColor = runtimeGridLineColor;
        lr.startWidth = runtimeLineWidth;
        lr.endWidth = runtimeLineWidth;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.sortingOrder = 1;
        
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        
        runtimeLines.Add(lineObj);
    }
    
    Material CreateDefaultLineMaterial()
    {
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = runtimeGridLineColor;
        return mat;
    }
    
    void DestroyRuntimeGridLines()
    {
        foreach (GameObject line in runtimeLines)
        {
            if (line != null)
            {
                if (Application.isPlaying)
                    Destroy(line);
                else
                    DestroyImmediate(line);
            }
        }
        runtimeLines.Clear();
    }
    
    void DrawFrame()
    {
        Gizmos.color = frameColor;
        
        Vector3 origin = transform.position + frameOffset;
        float width = gridSize.x * cellSize;
        float height = gridSize.y * cellSize;
        
        // Çerçeve köşeleri
        Vector3 bottomLeft = origin;
        Vector3 bottomRight = origin + new Vector3(width, 0, 0);
        Vector3 topLeft = origin + new Vector3(0, 0, height);
        Vector3 topRight = origin + new Vector3(width, 0, height);
        
        // Çerçeve çizgileri
        Gizmos.DrawLine(bottomLeft, bottomRight);  // Alt
        Gizmos.DrawLine(bottomRight, topRight);    // Sağ
        Gizmos.DrawLine(topRight, topLeft);        // Üst
        Gizmos.DrawLine(topLeft, bottomLeft);      // Sol
        
        // Kalın çizgi efekti
        for (int i = 1; i <= Mathf.RoundToInt(frameLineWidth * 20); i++)
        {
            float offset = i * 0.01f;
            
            // İç çizgiler
            Vector3 inOffset = new Vector3(offset, 0, offset);
            Gizmos.DrawLine(bottomLeft + inOffset, bottomRight + new Vector3(-offset, 0, offset));
            Gizmos.DrawLine(bottomRight + new Vector3(-offset, 0, offset), topRight + new Vector3(-offset, 0, -offset));
            Gizmos.DrawLine(topRight + new Vector3(-offset, 0, -offset), topLeft + new Vector3(offset, 0, -offset));
            Gizmos.DrawLine(topLeft + new Vector3(offset, 0, -offset), bottomLeft + inOffset);
        }
    }
    
    void DrawGizmoGridLines()
    {
        Color lineColor = gizmoGridLineColor;
        lineColor.a = gizmoGridLineAlpha;
        Gizmos.color = lineColor;
        
        Vector3 origin = transform.position + frameOffset;
        float width = gridSize.x * cellSize;
        float height = gridSize.y * cellSize;
        
        // Dikey çizgiler
        for (int x = 1; x < gridSize.x; x++)
        {
            Vector3 start = origin + new Vector3(x * cellSize, 0, 0);
            Vector3 end = start + new Vector3(0, 0, height);
            Gizmos.DrawLine(start, end);
        }
        
        // Yatay çizgiler
        for (int z = 1; z < gridSize.y; z++)
        {
            Vector3 start = origin + new Vector3(0, 0, z * cellSize);
            Vector3 end = start + new Vector3(width, 0, 0);
            Gizmos.DrawLine(start, end);
        }
    }
    
    void DrawCorners()
    {
        Gizmos.color = cornerColor;
        
        Vector3 origin = transform.position + frameOffset;
        float width = gridSize.x * cellSize;
        float height = gridSize.y * cellSize;
        
        // Köşe noktaları
        Vector3[] corners = {
            origin,                                    // Bottom Left
            origin + new Vector3(width, 0, 0),       // Bottom Right
            origin + new Vector3(0, 0, height),      // Top Left
            origin + new Vector3(width, 0, height)   // Top Right
        };
        
        foreach (Vector3 corner in corners)
        {
            Gizmos.DrawWireCube(corner, Vector3.one * cornerSize);
        }
    }
    
    // Public Methods
    public Vector2Int GetGridSize()
    {
        return gridSize;
    }
    
    public void SetGridSize(int width, int height)
    {
        gridSize = new Vector2Int(width, height);
    }
    
    public void SetGridSize(Vector2Int newSize)
    {
        gridSize = newSize;
    }
    
    public float GetCellSize()
    {
        return cellSize;
    }
    
    public void SetCellSize(float size)
    {
        cellSize = size;
    }
    
    public Vector3 GetFrameCenter()
    {
        Vector3 origin = transform.position + frameOffset;
        float width = gridSize.x * cellSize;
        float height = gridSize.y * cellSize;
        return origin + new Vector3(width * 0.5f, 0, height * 0.5f);
    }
    
    public Vector3 GetFrameOrigin()
    {
        return transform.position + frameOffset;
    }
    
    public bool IsPositionInFrame(Vector3 worldPosition)
    {
        Vector3 origin = GetFrameOrigin();
        Vector3 localPos = worldPosition - origin;
        
        return localPos.x >= 0 && localPos.x <= gridSize.x * cellSize &&
               localPos.z >= 0 && localPos.z <= gridSize.y * cellSize;
    }
    
    public Vector2Int WorldPositionToGridCell(Vector3 worldPosition)
    {
        Vector3 origin = GetFrameOrigin();
        Vector3 localPos = worldPosition - origin;
        
        int x = Mathf.FloorToInt(localPos.x / cellSize);
        int z = Mathf.FloorToInt(localPos.z / cellSize);
        
        return new Vector2Int(x, z);
    }
    
    // Runtime Grid Lines Control
    public void ToggleRuntimeGridLines(bool show)
    {
        showRuntimeGridLines = show;
        
        if (show)
        {
            CreateRuntimeGridLines();
        }
        else
        {
            DestroyRuntimeGridLines();
        }
    }
    
    public void SetRuntimeLineColor(Color color)
    {
        runtimeGridLineColor = color;
        
        // Mevcut çizgilerin rengini güncelle
        foreach (GameObject line in runtimeLines)
        {
            if (line != null)
            {
                LineRenderer lr = line.GetComponent<LineRenderer>();
                if (lr != null)
                {
                    lr.startColor = color;
                    lr.endColor = color;
                }
            }
        }
    }
}