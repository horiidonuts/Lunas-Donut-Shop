using UnityEngine;

public class SimpleGridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Grid grid;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private string gridPlaneTag = "GridPlane";
    
    [Header("Visual Indicators")]
    [SerializeField] private GameObject cellIndicator;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationSpeed = 5f;
    [SerializeField] private float targetHeight = 0.1f;
    
    [Header("Input")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask groundLayer = 1;
    
    private Vector3 mouseWorldPosition;
    private Vector3Int gridCellPosition;
    private Vector3Int previousGridCellPosition;
    private Vector3 gridWorldPosition;
    private Vector3 targetPosition;
    private Vector3 currentIndicatorPosition;
    private GameObject currentPlane; // Hangi plane üzerindeyiz
    
    void Start()
    {
        // Camera yoksa Main Camera'yı bul
        if (playerCamera == null)
            playerCamera = Camera.main;
            
        // Grid yoksa oluştur
        if (grid == null)
        {
            grid = GetComponent<Grid>();
            if (grid == null)
            {
                grid = gameObject.AddComponent<Grid>();
                grid.cellSize = Vector3.one * gridSize;
            }
        }
            
        // Cell indicator'ın collider'ını kaldır veya ignore layer'a al
        if (cellIndicator != null)
        {
            Collider indicatorCollider = cellIndicator.GetComponent<Collider>();
            if (indicatorCollider != null)
            {
                indicatorCollider.enabled = false;
            }
            
            // Başlangıç pozisyonunu ayarla
            currentIndicatorPosition = cellIndicator.transform.position;
        }
        
        previousGridCellPosition = Vector3Int.one * -999; // Invalid başlangıç değeri
    }
    
    void Update()
    {
        UpdateMousePosition();
        UpdateGridPosition();
        UpdateCellIndicator();
    }
    
    void UpdateMousePosition()
    {
        // Mouse pozisyonunu world space'e çevir
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = playerCamera.ScreenPointToRay(mouseScreenPos);
        
        // Tüm raycast hit'lerini al (duvarın arkasındaki objeleri de bul)
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, groundLayer);
        
        // GridPlane tag'ine sahip en yakın objeyi bul
        GameObject foundPlane = null;
        float closestDistance = float.MaxValue;
        Vector3 bestHitPoint = Vector3.zero;
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag(gridPlaneTag))
            {
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    foundPlane = hit.collider.gameObject;
                    bestHitPoint = hit.point;
                }
            }
        }
        
        if (foundPlane != null)
        {
            mouseWorldPosition = bestHitPoint;
            currentPlane = foundPlane;
        }
        else
        {
            currentPlane = null;
        }
    }
    
    void UpdateGridPosition()
    {
        // GridPlane tag'li plane üzerinde değilsek return
        if (currentPlane == null)
        {
            // CellIndicator'ı gizle
            if (cellIndicator != null)
                cellIndicator.SetActive(false);
            return;
        }
        
        // CellIndicator'ı göster
        if (cellIndicator != null)
            cellIndicator.SetActive(true);
        
        // Unity Grid ile basit snap
        gridCellPosition = grid.WorldToCell(mouseWorldPosition);
        gridWorldPosition = grid.CellToWorld(gridCellPosition);
        
        // Grid center'a getir
        gridWorldPosition += grid.cellSize * 0.5f;
        
        // Y pozisyonunu plane yüksekliğine ayarla
        gridWorldPosition.y = currentPlane.transform.position.y;
        
        // Yeni grid cell'e geçildiyse animasyonu resetle
        if (gridCellPosition != previousGridCellPosition)
        {
            // Target pozisyonunu plane'in üstünde ayarla
            targetPosition = new Vector3(gridWorldPosition.x, gridWorldPosition.y + targetHeight, gridWorldPosition.z);
            
            // Aşağıdan başlat
            currentIndicatorPosition = new Vector3(gridWorldPosition.x, gridWorldPosition.y - 0.5f, gridWorldPosition.z);
            
            previousGridCellPosition = gridCellPosition;
        }
        else
        {
            // Aynı cell'de, target pozisyonunu güncelle
            targetPosition = new Vector3(gridWorldPosition.x, gridWorldPosition.y + targetHeight, gridWorldPosition.z);
        }
    }
    
    void UpdateCellIndicator()
    {
        // GridPlane tag'li plane üzerinde değilsek return
        if (cellIndicator != null && currentPlane != null)
        {
            // Smooth interpolation ile yukarı çık
            currentIndicatorPosition = Vector3.Lerp(currentIndicatorPosition, targetPosition, animationSpeed * Time.deltaTime);
            cellIndicator.transform.position = currentIndicatorPosition;
        }
    }
    
    // Public method'lar
    public Vector3 GetGridPosition()
    {
        return gridWorldPosition;
    }
    
    public Vector3Int GetGridCellPosition()
    {
        return gridCellPosition;
    }
    
    public Vector3 GetMouseWorldPosition()
    {
        return mouseWorldPosition;
    }
    
    public GameObject GetCurrentPlane()
    {
        return currentPlane;
    }
    
    public bool IsOnValidPlane()
    {
        return currentPlane != null;
    }
}