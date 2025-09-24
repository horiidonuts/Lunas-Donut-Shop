using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    [Header("Grid Display Settings")]
    [SerializeField] private bool showGrid = true;
    [SerializeField] private int gridWidth = 20;
    [SerializeField] private int gridHeight = 20;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Color gridColor = Color.white;
    [SerializeField] private float gridAlpha = 0.3f;
    
    [Header("Grid Center")]
    [SerializeField] private Transform gridCenter;
    
    [Header("Materials")]
    [SerializeField] private Material gridLineMaterial;
    
    private LineRenderer[] horizontalLines;
    private LineRenderer[] verticalLines;
    private bool isInitialized = false;
    
    private void Start()
    {
        InitializeGrid();
    }
    
    private void InitializeGrid()
    {
        if (isInitialized) return;
        
        CreateGridLines();
        UpdateGridVisibility();
        isInitialized = true;
    }
    
    private void CreateGridLines()
    {
        GameObject gridParent = new GameObject("Grid Visualizer");
        gridParent.transform.parent = transform;
        
        Vector3 centerPos = gridCenter != null ? gridCenter.position : transform.position;
        
        // Horizontal lines
        horizontalLines = new LineRenderer[gridHeight + 1];
        for (int i = 0; i <= gridHeight; i++)
        {
            GameObject lineObj = new GameObject($"Horizontal Line {i}");
            lineObj.transform.parent = gridParent.transform;
            
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            SetupLineRenderer(lr);
            
            float z = centerPos.z + (i - gridHeight * 0.5f) * gridSize;
            float startX = centerPos.x - (gridWidth * 0.5f * gridSize);
            float endX = centerPos.x + (gridWidth * 0.5f * gridSize);
            
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(startX, centerPos.y, z));
            lr.SetPosition(1, new Vector3(endX, centerPos.y, z));
            
            horizontalLines[i] = lr;
        }
        
        // Vertical lines
        verticalLines = new LineRenderer[gridWidth + 1];
        for (int i = 0; i <= gridWidth; i++)
        {
            GameObject lineObj = new GameObject($"Vertical Line {i}");
            lineObj.transform.parent = gridParent.transform;
            
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            SetupLineRenderer(lr);
            
            float x = centerPos.x + (i - gridWidth * 0.5f) * gridSize;
            float startZ = centerPos.z - (gridHeight * 0.5f * gridSize);
            float endZ = centerPos.z + (gridHeight * 0.5f * gridSize);
            
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(x, centerPos.y, startZ));
            lr.SetPosition(1, new Vector3(x, centerPos.y, endZ));
            
            verticalLines[i] = lr;
        }
    }
    
    private void SetupLineRenderer(LineRenderer lr)
    {
        lr.material = gridLineMaterial != null ? gridLineMaterial : CreateDefaultMaterial();
        lr.startColor = new Color(gridColor.r, gridColor.g, gridColor.b, gridAlpha);
        lr.endColor = new Color(gridColor.r, gridColor.g, gridColor.b, gridAlpha);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.useWorldSpace = true;
        lr.sortingOrder = -1;
    }
    
    private Material CreateDefaultMaterial()
    {
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = Color.white;
        return mat;
    }
    
    public void UpdateGridVisibility()
    {
        if (!isInitialized) return;
        
        if (horizontalLines != null)
        {
            foreach (var line in horizontalLines)
            {
                if (line != null)
                    line.gameObject.SetActive(showGrid);
            }
        }
        
        if (verticalLines != null)
        {
            foreach (var line in verticalLines)
            {
                if (line != null)
                    line.gameObject.SetActive(showGrid);
            }
        }
    }
    
    public void SetGridColor(Color color)
    {
        gridColor = color;
        UpdateGridAppearance();
    }
    
    public void SetGridAlpha(float alpha)
    {
        gridAlpha = Mathf.Clamp01(alpha);
        UpdateGridAppearance();
    }
    
    private void UpdateGridAppearance()
    {
        if (!isInitialized) return;
        
        Color newColor = new Color(gridColor.r, gridColor.g, gridColor.b, gridAlpha);
        
        if (horizontalLines != null)
        {
            foreach (var line in horizontalLines)
            {
                if (line != null)
                {
                    line.startColor = newColor;
                    line.endColor = newColor;
                }
            }
        }
        
        if (verticalLines != null)
        {
            foreach (var line in verticalLines)
            {
                if (line != null)
                {
                    line.startColor = newColor;
                    line.endColor = newColor;
                }
            }
        }
    }
    
    public void ToggleGrid()
    {
        showGrid = !showGrid;
        UpdateGridVisibility();
    }
    
    // Public API
    public void SetGridSize(float size)
    {
        gridSize = size;
        // Grid'i yeniden oluştur
        DestroyGrid();
        InitializeGrid();
    }
    
    public void SetGridDimensions(int width, int height)
    {
        gridWidth = width;
        gridHeight = height;
        // Grid'i yeniden oluştur
        DestroyGrid();
        InitializeGrid();
    }
    
    private void DestroyGrid()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                    Destroy(transform.GetChild(i).gameObject);
                else
                    DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        
        horizontalLines = null;
        verticalLines = null;
        isInitialized = false;
    }
    
    private void OnValidate()
    {
        if (Application.isPlaying && isInitialized)
        {
            UpdateGridVisibility();
            UpdateGridAppearance();
        }
    }
    
    private void OnDestroy()
    {
        DestroyGrid();
    }
}