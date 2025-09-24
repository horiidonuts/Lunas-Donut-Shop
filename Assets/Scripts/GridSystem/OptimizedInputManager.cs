using UnityEngine;

public class OptimizedInputManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera sceneCamera;
    
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask placementLayerMask = 1;
    [SerializeField] private float maxRaycastDistance = 100f;
    
    [Header("Performance Settings")]
    [SerializeField] private bool useMouseDeltaOptimization = true;
    [SerializeField] private float mouseDeltaThreshold = 1f;
    
    // Cache variables
    private Vector3 lastMousePosition;
    private Vector3 lastWorldPosition;
    private bool hasValidPosition = false;
    
    // Performance tracking
    private float lastUpdateTime;
    
    private void Start()
    {
        if (sceneCamera == null)
            sceneCamera = Camera.main;
            
        if (sceneCamera == null)
            Debug.LogError("Scene camera is not assigned!");
    }
    
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 currentMousePos = Input.mousePosition;
        
        // Mouse hareketi yoksa cached pozisyonu döndür
        if (useMouseDeltaOptimization && hasValidPosition)
        {
            float mouseDelta = Vector3.Distance(currentMousePos, lastMousePosition);
            if (mouseDelta < mouseDeltaThreshold)
            {
                return lastWorldPosition;
            }
        }
        
        // Raycast yap
        Vector3 worldPos = PerformRaycast(currentMousePos);
        
        // Cache'i güncelle
        lastMousePosition = currentMousePos;
        if (worldPos != Vector3.zero)
        {
            lastWorldPosition = worldPos;
            hasValidPosition = true;
        }
        
        return hasValidPosition ? lastWorldPosition : Vector3.zero;
    }
    
    private Vector3 PerformRaycast(Vector3 mousePosition)
    {
        if (sceneCamera == null)
            return Vector3.zero;
        
        Ray ray = sceneCamera.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance, placementLayerMask))
        {
            return hit.point;
        }
        
        return Vector3.zero;
    }
    
    // Alternative method for specific world height
    public Vector3 GetSelectedMapPositionAtHeight(float height)
    {
        Vector3 currentMousePos = Input.mousePosition;
        
        if (sceneCamera == null)
            return Vector3.zero;
        
        // Mouse pozisyonunu world space'e çevir
        Vector3 mouseWorldPos = sceneCamera.ScreenToWorldPoint(
            new Vector3(currentMousePos.x, currentMousePos.y, sceneCamera.WorldToScreenPoint(Vector3.up * height).z)
        );
        
        return new Vector3(mouseWorldPos.x, height, mouseWorldPos.z);
    }
    
    // Plane intersection method (daha performanslı)
    public Vector3 GetSelectedMapPositionOnPlane(Plane plane)
    {
        Vector3 currentMousePos = Input.mousePosition;
        
        if (sceneCamera == null)
            return Vector3.zero;
        
        Ray ray = sceneCamera.ScreenPointToRay(currentMousePos);
        
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        
        return hasValidPosition ? lastWorldPosition : Vector3.zero;
    }
    
    // Grid snapping utility
    public Vector3 GetSnappedPosition(float snapSize)
    {
        Vector3 worldPos = GetSelectedMapPosition();
        
        return new Vector3(
            Mathf.Round(worldPos.x / snapSize) * snapSize,
            worldPos.y,
            Mathf.Round(worldPos.z / snapSize) * snapSize
        );
    }
    
    // Input state queries
    public bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current != null &&
               UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
    
    public bool IsPlacementInputPressed()
    {
        return Input.GetMouseButtonDown(0) && !IsMouseOverUI();
    }
    
    public bool IsRemovalInputPressed()
    {
        return Input.GetMouseButtonDown(1) && !IsMouseOverUI();
    }
    
    public bool IsCancelInputPressed()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }
    
    // Getters
    public Camera GetSceneCamera() => sceneCamera;
    public bool HasValidPosition() => hasValidPosition;
    public Vector3 GetLastValidPosition() => lastWorldPosition;
}