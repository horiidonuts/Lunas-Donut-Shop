using UnityEngine;

public class SimpleGridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Grid grid;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private string gridPlaneTag = "GridPlane";
    [SerializeField] private Vector3 gridOriginOffset = Vector3.zero;
    [SerializeField] private bool useCustomOrigin = false;
    
    [Header("Visual Indicators")]
    [SerializeField] private GameObject cellIndicator;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationSpeed = 5f;
    [SerializeField] private float targetHeight = 0.1f;
    
    [Header("Debug & Gizmos")]
    [SerializeField] private bool showGridOrigin = true;
    [SerializeField] private Color gridOriginColor = Color.red;
    [SerializeField] private float originGizmoSize = 0.5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask groundLayer = 1;
    
    private Vector3 mouseWorldPosition;
    private Vector3Int gridCellPosition;
    private Vector3Int previousGridCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
    private Vector3 gridWorldPosition;
    private Vector3 targetPosition;
    private Vector3 currentIndicatorPosition;
    private GameObject currentPlane; // Hangi plane üzerindeyiz
    private bool needsIndicatorUpdate;
    
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
        
        // Custom origin kullanılıyorsa grid transform'unu ayarla
        if (useCustomOrigin)
        {
            transform.position = gridOriginOffset;
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
        
        // Invalid başlangıç değeri artık constructor'da ayarlandı
        needsIndicatorUpdate = true;
    }
    
    void Update()
    {
        UpdateMousePosition();
        UpdateGridPosition();
        
        // Sadece gerektiğinde indicator'ı güncelle
        if (needsIndicatorUpdate)
            UpdateCellIndicator();
    }
    
    void UpdateMousePosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = playerCamera.ScreenPointToRay(mouseScreenPos);
        
        // Tek raycast ile ilk GridPlane'i bul (daha performanslı)
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            if (hit.collider.gameObject.CompareTag(gridPlaneTag))
            {
                mouseWorldPosition = hit.point;
                currentPlane = hit.collider.gameObject;
                return;
            }
        }
        
        currentPlane = null;
    }
    
    void UpdateGridPosition()
    {
        // GridPlane tag'li plane üzerinde değilsek return
        if (currentPlane == null)
        {
            // CellIndicator'ı gizle
            if (cellIndicator != null && cellIndicator.activeInHierarchy)
            {
                cellIndicator.SetActive(false);
                needsIndicatorUpdate = false;
            }
            return;
        }
        
        // CellIndicator'ı göster
        if (cellIndicator != null && !cellIndicator.activeInHierarchy)
            cellIndicator.SetActive(true);
        
        // Unity Grid ile basit snap
        Vector3 adjustedMousePosition = mouseWorldPosition;
        
        // Custom origin kullanılıyorsa mouse pozisyonunu grid origin'e göre ayarla
        if (useCustomOrigin)
        {
            adjustedMousePosition -= gridOriginOffset;
        }
        
        Vector3Int newGridCellPosition = grid.WorldToCell(adjustedMousePosition);
        
        // Sadece pozisyon değiştiyse güncelle
        if (newGridCellPosition != gridCellPosition)
        {
            gridCellPosition = newGridCellPosition;
            gridWorldPosition = grid.CellToWorld(gridCellPosition);
            
            // Custom origin kullanılıyorsa grid pozisyonunu offset ile ayarla
            if (useCustomOrigin)
            {
                gridWorldPosition += gridOriginOffset;
            }
            
            // Grid center'a getir
            gridWorldPosition += grid.cellSize * 0.5f;
            
            // Y pozisyonunu plane yüksekliğine ayarla
            gridWorldPosition.y = currentPlane.transform.position.y;
            
            // Target pozisyonunu hesapla
            targetPosition = new Vector3(gridWorldPosition.x, gridWorldPosition.y + targetHeight, gridWorldPosition.z);
            
            // Yeni grid cell'e geçildiyse animasyonu resetle
            if (gridCellPosition != previousGridCellPosition)
            {
                // Aşağıdan başlat
                currentIndicatorPosition = new Vector3(gridWorldPosition.x, gridWorldPosition.y - 0.5f, gridWorldPosition.z);
                previousGridCellPosition = gridCellPosition;
            }
            
            needsIndicatorUpdate = true;
        }
    }
    
    void UpdateCellIndicator()
    {
        if (cellIndicator != null && currentPlane != null)
        {
            // Smooth interpolation ile yukarı çık
            currentIndicatorPosition = Vector3.Lerp(currentIndicatorPosition, targetPosition, animationSpeed * Time.deltaTime);
            cellIndicator.transform.position = currentIndicatorPosition;
            
            // Hedefe ulaştıysa güncellemeyi durdur
            if (Vector3.Distance(currentIndicatorPosition, targetPosition) < 0.01f)
            {
                cellIndicator.transform.position = targetPosition;
                currentIndicatorPosition = targetPosition;
                needsIndicatorUpdate = false;
            }
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
    
    // Grid Origin Ayarlama Method'ları
    public void SetGridOrigin(Vector3 newOrigin)
    {
        gridOriginOffset = newOrigin;
        useCustomOrigin = true;
        transform.position = gridOriginOffset;
    }
    
    public void SetGridOriginToCurrentPosition()
    {
        gridOriginOffset = transform.position;
        useCustomOrigin = true;
    }
    
    public void ResetGridOrigin()
    {
        gridOriginOffset = Vector3.zero;
        useCustomOrigin = false;
        transform.position = Vector3.zero;
    }
    
    public Vector3 GetGridOrigin()
    {
        return useCustomOrigin ? gridOriginOffset : Vector3.zero;
    }
    
    public bool IsUsingCustomOrigin()
    {
        return useCustomOrigin;
    }
    
    // Scene'de grid origin'i göster
    void OnDrawGizmos()
    {
        if (showGridOrigin && useCustomOrigin)
        {
            Gizmos.color = gridOriginColor;
            Vector3 originPos = GetGridOrigin();
            
            // Cross çiz
            Gizmos.DrawLine(originPos + Vector3.left * originGizmoSize, originPos + Vector3.right * originGizmoSize);
            Gizmos.DrawLine(originPos + Vector3.forward * originGizmoSize, originPos + Vector3.back * originGizmoSize);
            Gizmos.DrawLine(originPos + Vector3.up * originGizmoSize, originPos + Vector3.down * originGizmoSize);
            
            // Küçük küp çiz
            Gizmos.DrawWireCube(originPos, Vector3.one * originGizmoSize * 0.3f);
        }
    }
}