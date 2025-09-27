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
        
        if (useCustomOrigin)
        {
            // Custom origin kullanıldığında tek büyük grid oluştur
            CreateSingleGridForAllPlanes(gridPlanes);
        }
        else
        {
            // Normal mod: Her plane için ayrı grid
            foreach (GameObject plane in gridPlanes)
            {
                CreateGridLinesForPlane(plane);
            }
        }
        
        Debug.Log($"Grid çizgileri {gridPlanes.Length} plane için oluşturuldu. Custom Origin: {useCustomOrigin}");
    }
    
    void CreateSingleGridForAllPlanes(GameObject[] planes)
    {
        if (planes.Length == 0) return;
        
        // Grid origin
        Vector3 gridOrigin = transform.position + customOriginOffset;
        
        // Tek parent object oluştur
        GameObject gridParent = new GameObject("CombinedGridLines");
        gridParent.transform.SetParent(transform);
        
        List<GameObject> allLines = new List<GameObject>();
        
        // Her plane için sadece o plane üstündeki çizgileri oluştur
        foreach (GameObject plane in planes)
        {
            Bounds planeBounds = GetPlaneBounds(plane);
            CreateGridLinesForPlaneBounds(planeBounds, gridOrigin, gridParent.transform, allLines);
        }
        
        // Tüm plane'ler için aynı line listesini kaydet
        foreach (GameObject plane in planes)
        {
            planeGridLines[plane] = allLines;
        }
    }
    
    void CreateGridLinesForPlaneBounds(Bounds planeBounds, Vector3 gridOrigin, Transform parent, List<GameObject> allLines)
    {
        // Plane'in gerçek sınırları
        float minX = planeBounds.min.x;
        float maxX = planeBounds.max.x;
        float minZ = planeBounds.min.z;
        float maxZ = planeBounds.max.z;
        float planeY = planeBounds.center.y;
        
        // Grid origin'e göre start/end grid koordinatları
        int startX = Mathf.FloorToInt((minX - gridOrigin.x) / gridCellSize);
        int endX = Mathf.CeilToInt((maxX - gridOrigin.x) / gridCellSize);
        int startZ = Mathf.FloorToInt((minZ - gridOrigin.z) / gridCellSize);
        int endZ = Mathf.CeilToInt((maxZ - gridOrigin.z) / gridCellSize);
        
        // Dikey çizgiler (X ekseni boyunca)
        for (int x = startX; x <= endX; x++)
        {
            float lineX = gridOrigin.x + x * gridCellSize;
            
            // Çizgiyi plane bounds içine kırp
            Vector3 lineStart = new Vector3(lineX, planeY + lineHeight, Mathf.Max(minZ, gridOrigin.z + startZ * gridCellSize));
            Vector3 lineEnd = new Vector3(lineX, planeY + lineHeight, Mathf.Min(maxZ, gridOrigin.z + endZ * gridCellSize));
            
            // Çizgi plane içinde mi kontrol et
            if (lineX >= minX && lineX <= maxX && lineStart.z < lineEnd.z)
            {
                string lineName = $"VerticalLine_X{x}_Plane{planeBounds.center.x:F1}_{planeBounds.center.z:F1}";
                GameObject line = CreateLine(lineName, lineStart, lineEnd, parent);
                allLines.Add(line);
            }
        }
        
        // Yatay çizgiler (Z ekseni boyunca)
        for (int z = startZ; z <= endZ; z++)
        {
            float lineZ = gridOrigin.z + z * gridCellSize;
            
            // Çizgiyi plane bounds içine kırp
            Vector3 lineStart = new Vector3(Mathf.Max(minX, gridOrigin.x + startX * gridCellSize), planeY + lineHeight, lineZ);
            Vector3 lineEnd = new Vector3(Mathf.Min(maxX, gridOrigin.x + endX * gridCellSize), planeY + lineHeight, lineZ);
            
            // Çizgi plane içinde mi kontrol et
            if (lineZ >= minZ && lineZ <= maxZ && lineStart.x < lineEnd.x)
            {
                string lineName = $"HorizontalLine_Z{z}_Plane{planeBounds.center.x:F1}_{planeBounds.center.z:F1}";
                GameObject line = CreateLine(lineName, lineStart, lineEnd, parent);
                allLines.Add(line);
            }
        }
    }
    
    Bounds GetPlaneBounds(GameObject plane)
    {
        Collider planeCollider = plane.GetComponent<Collider>();
        if (planeCollider != null)
        {
            return planeCollider.bounds;
        }
        
        // Collider yoksa transform pozisyonundan varsayılan boyut
        return new Bounds(plane.transform.position, Vector3.one * gridCellSize);
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
            
            // Custom origin modunda aynı line'lar birden fazla plane'de olabilir
            // Sadece ilk plane line'ları silsin
            bool shouldDestroyLines = true;
            if (useCustomOrigin)
            {
                // Bu line'ları kullanan başka plane var mı kontrol et
                foreach (var kvp in planeGridLines)
                {
                    if (kvp.Key != plane && kvp.Value == lines)
                    {
                        shouldDestroyLines = false;
                        break;
                    }
                }
            }
            
            if (shouldDestroyLines)
            {
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
                
                // Parent object'i de sil (CombinedGridLines)
                if (lines.Count > 0 && lines[0] != null && lines[0].transform.parent != null)
                {
                    GameObject parent = lines[0].transform.parent.gameObject;
                    if (parent.name.Contains("CombinedGridLines"))
                    {
                        if (Application.isPlaying)
                            Destroy(parent);
                        else
                            DestroyImmediate(parent);
                    }
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