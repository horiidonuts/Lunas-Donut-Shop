using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BuildableObject
{
    public string name;
    public GameObject prefab;
    public Vector2Int size = Vector2Int.one; // Grid boyutu (width x height)
    public int cost;
    public Sprite icon;
    public bool canRotate = true;
}

public class BuildingSystem : MonoBehaviour
{
    [Header("Building Objects")]
    [SerializeField] private BuildableObject[] buildableObjects;
    
    [Header("System References")]
    [SerializeField] private ImprovedPlacementSystem placementSystem;
    [SerializeField] private OptimizedInputManager inputManager;
    
    [Header("Building Settings")]
    [SerializeField] private LayerMask buildingLayer = 1;
    [SerializeField] private bool allowOverlapping = false;
    
    // Current building state
    private int selectedObjectIndex = 0;
    private BuildableObject currentObject;
    private GameObject previewObject;
    private int currentRotation = 0; // 0, 90, 180, 270 degrees
    
    // Building data
    private Dictionary<Vector3Int, GameObject> placedObjects = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, BuildableObject> objectData = new Dictionary<Vector3Int, BuildableObject>();
    
    // Events
    public System.Action<BuildableObject> OnObjectSelected;
    public System.Action<Vector3Int, BuildableObject> OnObjectBuilt;
    public System.Action<Vector3Int, BuildableObject> OnObjectDestroyed;
    
    private void Start()
    {
        InitializeSystem();
    }
    
    private void Update()
    {
        HandleBuildingInput();
        UpdatePreviewObject();
    }
    
    private void InitializeSystem()
    {
        if (buildableObjects.Length > 0)
        {
            SelectObject(0);
        }
        
        // PlacementSystem event'lerini dinle
        if (placementSystem != null)
        {
            placementSystem.OnGridPositionChanged += OnGridPositionChanged;
        }
    }
    
    private void HandleBuildingInput()
    {
        // Object selection (1-9 keys)
        for (int i = 1; i <= 9 && i <= buildableObjects.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SelectObject(i - 1);
            }
        }
        
        // Rotation (R key)
        if (Input.GetKeyDown(KeyCode.R) && currentObject != null && currentObject.canRotate)
        {
            RotateCurrentObject();
        }
        
        // Building (Left mouse button)
        if (inputManager.IsPlacementInputPressed() && placementSystem.IsCurrentPlacementValid())
        {
            BuildCurrentObject();
        }
        
        // Demolish (Right mouse button)
        if (inputManager.IsRemovalInputPressed())
        {
            DemolishObject(placementSystem.GetCurrentGridPosition());
        }
        
        // Cancel building (Escape)
        if (inputManager.IsCancelInputPressed())
        {
            CancelBuilding();
        }
    }
    
    private void UpdatePreviewObject()
    {
        if (currentObject == null) return;
        
        Vector3Int gridPos = placementSystem.GetCurrentGridPosition();
        Vector3 worldPos = placementSystem.GridToWorldPosition(gridPos);
        
        // Preview object'i oluştur veya güncelle
        if (previewObject == null)
        {
            CreatePreviewObject();
        }
        
        if (previewObject != null)
        {
            previewObject.transform.position = worldPos;
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
            
            // Preview'in görünürlüğünü kontrol et
            bool isValid = placementSystem.IsCurrentPlacementValid() && CanPlaceObjectAtPosition(gridPos);
            SetPreviewVisibility(isValid);
        }
    }
    
    private void CreatePreviewObject()
    {
        if (currentObject?.prefab == null) return;
        
        previewObject = Instantiate(currentObject.prefab);
        previewObject.name = "Preview_" + currentObject.name;
        
        // Preview object'i interaktif olmaktan çıkar
        MakeObjectPreview(previewObject);
    }
    
    private void MakeObjectPreview(GameObject obj)
    {
        // Collider'ları deaktive et
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        // Renderer'larda transparency ekle
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    Color color = material.color;
                    color.a = 0.5f;
                    material.color = color;
                }
            }
        }
        
        // Tag'i preview yap
        obj.tag = "Preview";
    }
    
    private void SetPreviewVisibility(bool isValid)
    {
        if (previewObject == null) return;
        
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        Color targetColor = isValid ? Color.green : Color.red;
        targetColor.a = 0.5f;
        
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = targetColor;
                }
            }
        }
    }
    
    public void SelectObject(int index)
    {
        if (index < 0 || index >= buildableObjects.Length) return;
        
        selectedObjectIndex = index;
        currentObject = buildableObjects[index];
        currentRotation = 0;
        
        // Eski preview'i temizle
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
        
        OnObjectSelected?.Invoke(currentObject);
    }
    
    public void RotateCurrentObject()
    {
        currentRotation = (currentRotation + 90) % 360;
        
        // Preview'i güncelle
        if (previewObject != null)
        {
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
    }
    
    public void BuildCurrentObject()
    {
        if (currentObject?.prefab == null) return;
        
        Vector3Int gridPos = placementSystem.GetCurrentGridPosition();
        
        if (!CanPlaceObjectAtPosition(gridPos)) return;
        
        // Object'i inşa et
        Vector3 worldPos = placementSystem.GridToWorldPosition(gridPos);
        GameObject newBuilding = Instantiate(currentObject.prefab, worldPos, Quaternion.Euler(0, currentRotation, 0));
        newBuilding.name = currentObject.name + "_" + gridPos.ToString();
        
        // Veriyi kaydet
        placedObjects[gridPos] = newBuilding;
        objectData[gridPos] = currentObject;
        
        // Placement system'e bildir
        placementSystem.PlaceObject(gridPos);
        
        // Event tetikle
        OnObjectBuilt?.Invoke(gridPos, currentObject);
        
        Debug.Log($"Built {currentObject.name} at {gridPos}");
    }
    
    public void DemolishObject(Vector3Int gridPos)
    {
        if (placedObjects.ContainsKey(gridPos))
        {
            GameObject building = placedObjects[gridPos];
            BuildableObject data = objectData[gridPos];
            
            // Object'i yok et
            Destroy(building);
            placedObjects.Remove(gridPos);
            objectData.Remove(gridPos);
            
            // Placement system'den kaldır
            placementSystem.RemoveObject(gridPos);
            
            // Event tetikle
            OnObjectDestroyed?.Invoke(gridPos, data);
            
            Debug.Log($"Demolished {data.name} at {gridPos}");
        }
    }
    
    private bool CanPlaceObjectAtPosition(Vector3Int gridPos)
    {
        // Multi-cell objects için kontrol
        Vector2Int size = currentObject.size;
        
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int checkPos = gridPos + new Vector3Int(x, 0, y);
                
                // Hücre dolu mu?
                if (placedObjects.ContainsKey(checkPos))
                    return false;
                    
                // Placement system kontrolü
                if (!placementSystem.IsValidPlacement(checkPos))
                    return false;
            }
        }
        
        return true;
    }
    
    private void OnGridPositionChanged(Vector3Int gridPos, bool isValid)
    {
        // Grid pozisyonu değiştiğinde preview'i güncelle
        // Bu method UpdatePreviewObject() tarafından handle ediliyor
    }
    
    public void CancelBuilding()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
        
        currentObject = null;
    }
    
    // Public API
    public BuildableObject GetCurrentObject() => currentObject;
    public int GetCurrentRotation() => currentRotation;
    public bool IsBuilding() => currentObject != null;
    public Dictionary<Vector3Int, GameObject> GetPlacedObjects() => new Dictionary<Vector3Int, GameObject>(placedObjects);
    public BuildableObject[] GetAvailableObjects() => buildableObjects;
    
    // Save/Load için data export
    [System.Serializable]
    public class BuildingSaveData
    {
        public Vector3Int position;
        public string objectName;
        public int rotation;
    }
    
    public BuildingSaveData[] GetBuildingData()
    {
        List<BuildingSaveData> data = new List<BuildingSaveData>();
        
        foreach (var kvp in objectData)
        {
            data.Add(new BuildingSaveData
            {
                position = kvp.Key,
                objectName = kvp.Value.name,
                rotation = placedObjects[kvp.Key].transform.eulerAngles.y > 0 ? 
                          Mathf.RoundToInt(placedObjects[kvp.Key].transform.eulerAngles.y) : 0
            });
        }
        
        return data.ToArray();
    }
    
    private void OnDestroy()
    {
        if (previewObject != null)
            Destroy(previewObject);
    }
}