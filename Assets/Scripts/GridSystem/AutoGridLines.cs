using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AutoGridLines : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private float gridCellSize = 1f;
    [SerializeField] private string gridPlaneTag = "GridPlane";
    
    [Header("Grid Origin/Pivot")]
    [SerializeField] private bool useCustomOrigin = false;
    [SerializeField] private Vector3 customOriginOffset = Vector3.zero;
    [SerializeField] private bool showOriginGizmo = true;
    [SerializeField] private Color originGizmoColor = Color.red;
    [SerializeField] private float originGizmoSize = 0.5f;
    
    [Header("Line Visual")]
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Color gridLineColor = Color.white;
    [SerializeField] private float lineWidth = 0.02f;
    [SerializeField] private float lineHeight = 0.01f; // Plane üstündeki yükseklik
    
    [Header("Grid Size")]
    [SerializeField] private Vector2Int defaultGridSize = new Vector2Int(20, 20); // Varsayılan grid boyutu
    [SerializeField] private bool autoDetectPlaneSize = true; // Plane boyutunu otomatik algıla
    
    private Dictionary<GameObject, List<GameObject>> planeGridLines = new Dictionary<GameObject, List<GameObject>>();
    
    void Start()
    {
        // Tüm GridPlane tag'li objeleri bul ve grid çizgilerini oluştur
        CreateGridLinesForAllPlanes();
    }
    
    void CreateGridLinesForAllPlanes()
    {
        GameObject[] gridPlanes = GameObject.FindGameObjectsWithTag(gridPlaneTag);
        
        foreach (GameObject plane in gridPlanes)
        {
            CreateGridLinesForPlane(plane);
        }
        
        Debug.Log($"Grid çizgileri {gridPlanes.Length} plane için oluşturuldu.");
    }
    
    void CreateGridLinesForPlane(GameObject plane)
    {
        if (planeGridLines.ContainsKey(plane))
        {
            // Bu plane için zaten çizgiler var, önce sil
            DestroyGridLinesForPlane(plane);
        }
        
        Vector2Int gridSize = GetGridSizeForPlane(plane);
        Vector3 planeCenter = plane.transform.position;
        Vector3 gridOrigin = GetGridOriginForPlane(plane, gridSize);
        
        List<GameObject> lines = new List<GameObject>();
        
        // Dikey çizgiler
        for (int x = 0; x <= gridSize.x; x++)
        {
            Vector3 start = gridOrigin + new Vector3(x * gridCellSize, lineHeight, 0);
            Vector3 end = start + new Vector3(0, 0, gridSize.y * gridCellSize);
            
            GameObject line = CreateLine($"{plane.name}_VerticalLine_{x}", start, end, plane.transform);
            lines.Add(line);
        }
        
        // Yatay çizgiler
        for (int z = 0; z <= gridSize.y; z++)
        {
            Vector3 start = gridOrigin + new Vector3(0, lineHeight, z * gridCellSize);
            Vector3 end = start + new Vector3(gridSize.x * gridCellSize, 0, 0);
            
            GameObject line = CreateLine($"{plane.name}_HorizontalLine_{z}", start, end, plane.transform);
            lines.Add(line);
        }
        
        planeGridLines[plane] = lines;
    }
    
    Vector2Int GetGridSizeForPlane(GameObject plane)
    {
        if (autoDetectPlaneSize)
        {
            // Plane'in collider'ından boyut algıla
            Collider planeCollider = plane.GetComponent<Collider>();
            if (planeCollider != null)
            {
                Vector3 size = planeCollider.bounds.size;
                int gridX = Mathf.RoundToInt(size.x / gridCellSize);
                int gridZ = Mathf.RoundToInt(size.z / gridCellSize);
                return new Vector2Int(Mathf.Max(1, gridX), Mathf.Max(1, gridZ));
            }
        }
        
        return defaultGridSize;
    }
    
    Vector3 GetGridOriginForPlane(GameObject plane, Vector2Int gridSize)
    {
        if (useCustomOrigin)
        {
            // Custom origin kullan
            return transform.position + customOriginOffset;
        }
        else
        {
            // Grid'i plane'in merkezine hizala (eski davranış)
            Vector3 planeCenter = plane.transform.position;
            float gridWidth = gridSize.x * gridCellSize;
            float gridHeight = gridSize.y * gridCellSize;
            
            Vector3 origin = new Vector3(
                planeCenter.x - gridWidth * 0.5f,
                planeCenter.y,
                planeCenter.z - gridHeight * 0.5f
            );
            
            return origin;
        }
    }
    
    GameObject CreateLine(string name, Vector3 start, Vector3 end, Transform parent)
    {
        GameObject lineObj = new GameObject(name);
        lineObj.transform.SetParent(parent);
        
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        
        // Materyal ayarla
        if (lineMaterial != null)
        {
            lr.material = lineMaterial;
        }
        else
        {
            lr.material = CreateDefaultLineMaterial();
        }
        
        lr.startColor = gridLineColor;
        lr.endColor = gridLineColor;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.sortingOrder = 1;
        
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        
        return lineObj;
    }
    
    Material CreateDefaultLineMaterial()
    {
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = gridLineColor;
        return mat;
    }
    
    void DestroyGridLinesForPlane(GameObject plane)
    {
        if (planeGridLines.ContainsKey(plane))
        {
            List<GameObject> lines = planeGridLines[plane];
            foreach (GameObject line in lines)
            {
                if (line != null)
                {
                    if (Application.isPlaying)
                        Destroy(line);
                    else
                        DestroyImmediate(line);
                }
            }
            lines.Clear();
            planeGridLines.Remove(plane);
        }
    }
    
    void OnDestroy()
    {
        // Tüm grid çizgilerini temizle - ToArray() ile enumeration safe yap
        foreach (var kvp in planeGridLines.ToArray())
        {
            DestroyGridLinesForPlane(kvp.Key);
        }
        planeGridLines.Clear();
    }
    
    void OnDrawGizmos()
    {
        // Grid origin/pivot göster
        if (showOriginGizmo && useCustomOrigin)
        {
            Gizmos.color = originGizmoColor;
            Vector3 originPos = transform.position + customOriginOffset;
            
            // Cross çiz
            Gizmos.DrawLine(originPos + Vector3.left * originGizmoSize, originPos + Vector3.right * originGizmoSize);
            Gizmos.DrawLine(originPos + Vector3.forward * originGizmoSize, originPos + Vector3.back * originGizmoSize);
            Gizmos.DrawLine(originPos + Vector3.up * originGizmoSize, originPos + Vector3.down * originGizmoSize);
            
            // Küçük küp çiz
            Gizmos.DrawWireCube(originPos, Vector3.one * originGizmoSize * 0.3f);
        }
    }
    
    // Grid Origin/Pivot Method'ları
    public void SetGridOrigin(Vector3 newOrigin)
    {
        customOriginOffset = newOrigin - transform.position;
        useCustomOrigin = true;
        RefreshGridLines();
    }
    
    public void SetGridOriginToCurrentPosition()
    {
        customOriginOffset = Vector3.zero;
        useCustomOrigin = true;
        RefreshGridLines();
    }
    
    public void ResetGridOrigin()
    {
        customOriginOffset = Vector3.zero;
        useCustomOrigin = false;
        RefreshGridLines();
    }
    
    public Vector3 GetGridOrigin()
    {
        return useCustomOrigin ? (transform.position + customOriginOffset) : transform.position;
    }
    
    public bool IsUsingCustomOrigin()
    {
        return useCustomOrigin;
    }
    
    // Public Methods
    public void RefreshGridLines()
    {
        // Mevcut çizgileri sil ve yeniden oluştur
        foreach (var kvp in planeGridLines.ToArray())
        {
            DestroyGridLinesForPlane(kvp.Key);
        }
        CreateGridLinesForAllPlanes();
    }
    
    public void SetGridCellSize(float newSize)
    {
        gridCellSize = newSize;
        RefreshGridLines();
    }
    
    public void SetGridLineColor(Color newColor)
    {
        gridLineColor = newColor;
        
        // Mevcut çizgilerin rengini güncelle
        foreach (var lines in planeGridLines.Values)
        {
            foreach (GameObject line in lines)
            {
                if (line != null)
                {
                    LineRenderer lr = line.GetComponent<LineRenderer>();
                    if (lr != null)
                    {
                        lr.startColor = newColor;
                        lr.endColor = newColor;
                    }
                }
            }
        }
    }
    
    public void ToggleGridLines(bool show)
    {
        foreach (var lines in planeGridLines.Values)
        {
            foreach (GameObject line in lines)
            {
                if (line != null)
                {
                    line.SetActive(show);
                }
            }
        }
    }
}