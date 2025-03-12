using UnityEditor;
using UnityEngine;

public class SettingsBackButton : MonoBehaviour
{
    private SettingsButton _settingsButton;
    
    void Start()
    {
        _settingsButton = gameObject.GetComponent<SettingsButton>();
    }

    void Update()
    {
        
    }

    public void GoBack()
    {
        _settingsButton.MoveMenuButtons();
    }
}
