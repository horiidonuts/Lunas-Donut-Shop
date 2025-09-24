using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private Transform objectButtonParent;
    [SerializeField] private Button objectButtonPrefab;
    [SerializeField] private TextMeshProUGUI selectedObjectText;
    [SerializeField] private TextMeshProUGUI instructionsText;
    
    [Header("Info Display")]
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private TextMeshProUGUI objectCostText;
    [SerializeField] private Image objectIconImage;
    
    [Header("Controls Info")]
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private Button toggleControlsButton;
    
    [Header("System References")]
    [SerializeField] private BuildingSystem buildingSystem;
    
    private Button[] objectButtons;
    private int currentSelectedIndex = -1;
    
    private void Start()
    {
        InitializeUI();
        SetupEvents();
        UpdateUI();
    }
    
    private void InitializeUI()
    {
        if (buildingSystem == null)
            buildingSystem = FindObjectOfType<BuildingSystem>();
        
        CreateObjectButtons();
        SetupControlsPanel();
        
        // Instructions text
        if (instructionsText != null)
        {
            instructionsText.text = "Sol tık: İnşa et\nSağ tık: Yık\nR: Döndür\nESC: İptal\n1-9: Obje seç";
        }
    }
    
    private void CreateObjectButtons()
    {
        if (buildingSystem == null || objectButtonParent == null || objectButtonPrefab == null)
            return;
        
        BuildableObject[] objects = buildingSystem.GetAvailableObjects();
        objectButtons = new Button[objects.Length];
        
        for (int i = 0; i < objects.Length; i++)
        {
            BuildableObject obj = objects[i];
            int index = i; // Capture for lambda
            
            // Button oluştur
            Button button = Instantiate(objectButtonPrefab, objectButtonParent);
            button.name = "Button_" + obj.name;
            
            // Button'a click event ekle
            button.onClick.AddListener(() => SelectObject(index));
            
            // Button içeriğini ayarla
            SetupObjectButton(button, obj, index);
            
            objectButtons[i] = button;
        }
    }
    
    private void SetupObjectButton(Button button, BuildableObject obj, int index)
    {
        // Icon
        Image iconImage = button.transform.Find("Icon")?.GetComponent<Image>();
        if (iconImage != null && obj.icon != null)
        {
            iconImage.sprite = obj.icon;
        }
        
        // Name text
        TextMeshProUGUI nameText = button.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        if (nameText != null)
        {
            nameText.text = obj.name;
        }
        
        // Cost text
        TextMeshProUGUI costText = button.transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();
        if (costText != null)
        {
            costText.text = obj.cost.ToString();
        }
        
        // Hotkey text
        TextMeshProUGUI hotkeyText = button.transform.Find("Hotkey")?.GetComponent<TextMeshProUGUI>();
        if (hotkeyText != null)
        {
            hotkeyText.text = (index + 1).ToString();
        }
    }
    
    private void SetupControlsPanel()
    {
        if (toggleControlsButton != null)
        {
            toggleControlsButton.onClick.AddListener(ToggleControlsPanel);
        }
        
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }
    }
    
    private void SetupEvents()
    {
        if (buildingSystem != null)
        {
            buildingSystem.OnObjectSelected += OnObjectSelected;
            buildingSystem.OnObjectBuilt += OnObjectBuilt;
            buildingSystem.OnObjectDestroyed += OnObjectDestroyed;
        }
    }
    
    private void Update()
    {
        // Hotkey controls
        for (int i = 1; i <= 9 && i <= objectButtons.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SelectObject(i - 1);
            }
        }
        
        // Toggle building panel
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBuildingPanel();
        }
        
        // Toggle controls panel
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleControlsPanel();
        }
    }
    
    public void SelectObject(int index)
    {
        if (buildingSystem != null)
        {
            buildingSystem.SelectObject(index);
        }
    }
    
    private void OnObjectSelected(BuildableObject obj)
    {
        currentSelectedIndex = System.Array.FindIndex(buildingSystem.GetAvailableObjects(), x => x == obj);
        UpdateSelectedObjectDisplay(obj);
        UpdateButtonHighlights();
    }
    
    private void OnObjectBuilt(Vector3Int position, BuildableObject obj)
    {
        // Build sound effect veya animation eklenebilir
        Debug.Log($"UI: Built {obj.name} at {position}");
    }
    
    private void OnObjectDestroyed(Vector3Int position, BuildableObject obj)
    {
        // Destroy sound effect veya animation eklenebilir
        Debug.Log($"UI: Destroyed {obj.name} at {position}");
    }
    
    private void UpdateSelectedObjectDisplay(BuildableObject obj)
    {
        if (selectedObjectText != null)
        {
            selectedObjectText.text = "Seçili: " + obj.name;
        }
        
        if (objectNameText != null)
        {
            objectNameText.text = obj.name;
        }
        
        if (objectCostText != null)
        {
            objectCostText.text = "Maliyet: " + obj.cost;
        }
        
        if (objectIconImage != null && obj.icon != null)
        {
            objectIconImage.sprite = obj.icon;
        }
    }
    
    private void UpdateButtonHighlights()
    {
        for (int i = 0; i < objectButtons.Length; i++)
        {
            if (objectButtons[i] != null)
            {
                // Selected button'ı highlight et
                ColorBlock colors = objectButtons[i].colors;
                colors.normalColor = (i == currentSelectedIndex) ? Color.yellow : Color.white;
                objectButtons[i].colors = colors;
            }
        }
    }
    
    public void ToggleBuildingPanel()
    {
        if (buildingPanel != null)
        {
            buildingPanel.SetActive(!buildingPanel.activeSelf);
        }
    }
    
    public void ToggleControlsPanel()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(!controlsPanel.activeSelf);
        }
    }
    
    public void CancelBuilding()
    {
        if (buildingSystem != null)
        {
            buildingSystem.CancelBuilding();
        }
        
        currentSelectedIndex = -1;
        UpdateButtonHighlights();
        
        if (selectedObjectText != null)
        {
            selectedObjectText.text = "Seçili: Yok";
        }
    }
    
    private void UpdateUI()
    {
        // UI'ın genel durumunu güncelle
        if (buildingPanel != null)
        {
            buildingPanel.SetActive(true);
        }
    }
    
    // Public API
    public void ShowBuildingPanel() => buildingPanel?.SetActive(true);
    public void HideBuildingPanel() => buildingPanel?.SetActive(false);
    public bool IsBuildingPanelVisible() => buildingPanel != null && buildingPanel.activeSelf;
    
    private void OnDestroy()
    {
        // Event'leri temizle
        if (buildingSystem != null)
        {
            buildingSystem.OnObjectSelected -= OnObjectSelected;
            buildingSystem.OnObjectBuilt -= OnObjectBuilt;
            buildingSystem.OnObjectDestroyed -= OnObjectDestroyed;
        }
    }
}