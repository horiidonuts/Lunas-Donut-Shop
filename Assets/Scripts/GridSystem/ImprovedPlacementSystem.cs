using UnityEngine;
using System.Collections.Generic;

public class ImprovedPlacementSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Grid grid;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Vector3 gridOffset = Vector3.zero;
    
    [Header("Visual Indicators")]
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject validPlacementIndicator;
    [SerializeField] private GameObject invalidPlacementIndicator;
    
    [Header("Input")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Camera sceneCamera;
    
    [Header("Placement Settings")]
    [SerializeField] private LayerMask placementLayerMask = 1;
    [SerializeField] private LayerMask obstacleLayerMask = 0;
    [SerializeField] private float placementCheckRadius = 0.4f;
    
    // Grid state
    private Vector3Int currentGridPosition;
    private Vector3Int previousGridPosition;
    private Vector3 currentWorldPosition;
    private bool isValidPlacement = true;
    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();
    
    // Events
    public System.Action<Vector3Int, bool> OnGridPositionChanged;
    public System.Action<Vector3Int> OnObjectPlaced;
    
    private void Start()
    {
        InitializeGrid();
        SetupIndicators();
    }
    
    private void Update()
    {
        UpdateGridPosition();
        UpdateVisualFeedback();
        HandleInput();
    }
    
    private void InitializeGrid()
    {
        if (grid == null)
        {
            grid = FindObjectOfType<Grid>();
            if (grid == null)
            {
                Debug.LogWarning("Grid component not found! Creating default grid.");
                GameObject gridObj = new GameObject("Grid");
                grid = gridObj.AddComponent<Grid>();
                grid.cellSize = Vector3.one * gridSize;
            }
        }
    }
    
    private void SetupIndicators()
    {
        // Mouse indicator'ı sürekli aktif tut
        if (mouseIndicator != null)
            mouseIndicator.SetActive(true);
            
        // Cell indicator'ı başta gizle
        if (cellIndicator != null)
            cellIndicator.SetActive(false);
    }
    
    private void UpdateGridPosition()
    {
        Vector3 mouseWorldPos = inputManager.GetSelectedMapPosition();
        currentWorldPosition = mouseWorldPos;
        
        // Grid pozisyonunu hesapla
        Vector3Int newGridPosition = WorldToGridPosition(mouseWorldPos);
        
        // Grid pozisyonu değiştiyse
        if (newGridPosition != currentGridPosition)
        {
            previousGridPosition = currentGridPosition;
            currentGridPosition = newGridPosition;
            
            // Placement validasyonunu kontrol et
            isValidPlacement = IsValidPlacement(currentGridPosition);
            
            // Event'i tetikle
            OnGridPositionChanged?.Invoke(currentGridPosition, isValidPlacement);
        }
    }
    
    private void UpdateVisualFeedback()
    {
        // Mouse indicator pozisyonunu güncelle
        if (mouseIndicator != null)
        {
            mouseIndicator.transform.position = currentWorldPosition;
        }
        
        // Cell indicator pozisyonunu güncelle
        Vector3 cellWorldPos = GridToWorldPosition(currentGridPosition);
        
        if (cellIndicator != null)
        {
            cellIndicator.transform.position = cellWorldPos;
            cellIndicator.SetActive(true);
        }
        
        // Placement validity göstergeleri
        UpdatePlacementIndicators(cellWorldPos);
    }
    
    private void UpdatePlacementIndicators(Vector3 position)
    {
        if (validPlacementIndicator != null)
        {
            validPlacementIndicator.transform.position = position;
            validPlacementIndicator.SetActive(isValidPlacement);
        }
        
        if (invalidPlacementIndicator != null)
        {
            invalidPlacementIndicator.transform.position = position;
            invalidPlacementIndicator.SetActive(!isValidPlacement);
        }
    }
    
    private void HandleInput()
    {
        // Mouse sol tuş ile placement
        if (Input.GetMouseButtonDown(0) && isValidPlacement)
        {
            PlaceObject(currentGridPosition);
        }
        
        // Mouse sağ tuş ile object kaldırma
        if (Input.GetMouseButtonDown(1))
        {
            RemoveObject(currentGridPosition);
        }
    }
    
    private bool IsValidPlacement(Vector3Int gridPos)
    {
        // Hücre zaten dolu mu?
        if (occupiedCells.Contains(gridPos))
            return false;
        
        Vector3 worldPos = GridToWorldPosition(gridPos);
        
        // Obstacle kontrolü
        Collider[] obstacles = Physics.OverlapSphere(worldPos, placementCheckRadius, obstacleLayerMask);
        if (obstacles.Length > 0)
            return false;
        
        // Placement layer kontrolü
        Ray ray = new Ray(worldPos + Vector3.up * 10f, Vector3.down);
        if (!Physics.Raycast(ray, 20f, placementLayerMask))
            return false;
        
        return true;
    }
    
    public void PlaceObject(Vector3Int gridPos)
    {
        if (!IsValidPlacement(gridPos))
            return;
        
        // Hücreyi dolu olarak işaretle
        occupiedCells.Add(gridPos);
        
        // Event tetikle
        OnObjectPlaced?.Invoke(gridPos);
        
        Debug.Log($"Object placed at grid position: {gridPos}");
    }
    
    public void RemoveObject(Vector3Int gridPos)
    {
        if (occupiedCells.Contains(gridPos))
        {
            occupiedCells.Remove(gridPos);
            Debug.Log($"Object removed from grid position: {gridPos}");
        }
    }
    
    // Grid conversion methods
    public Vector3Int WorldToGridPosition(Vector3 worldPos)
    {
        Vector3 adjustedPos = worldPos - gridOffset;
        return Vector3Int.FloorToInt(adjustedPos / gridSize);
    }
    
    public Vector3 GridToWorldPosition(Vector3Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, gridPos.z) * gridSize + gridOffset + Vector3.one * gridSize * 0.5f;
    }
    
    // Utility methods
    public bool IsCellOccupied(Vector3Int gridPos)
    {
        return occupiedCells.Contains(gridPos);
    }
    
    public void ClearAllPlacements()
    {
        occupiedCells.Clear();
    }
    
    public Vector3Int GetCurrentGridPosition()
    {
        return currentGridPosition;
    }
    
    public Vector3 GetCurrentWorldPosition()
    {
        return currentWorldPosition;
    }
    
    public bool IsCurrentPlacementValid()
    {
        return isValidPlacement;
    }
    
    // Gizmos for debugging
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        // Current grid cell
        Gizmos.color = isValidPlacement ? Color.green : Color.red;
        Vector3 cellCenter = GridToWorldPosition(currentGridPosition);
        Gizmos.DrawWireCube(cellCenter, Vector3.one * gridSize);
        
        // Occupied cells
        Gizmos.color = Color.blue;
        foreach (var cell in occupiedCells)
        {
            Vector3 center = GridToWorldPosition(cell);
            Gizmos.DrawWireCube(center, Vector3.one * gridSize * 0.9f);
        }
    }
}