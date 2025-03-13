using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsBackButton : MonoBehaviour
{
    private MenuBg _menuBg;
    [SerializeField] private RawImage backgroundImage;
    

    private void Start()
    {
        if (backgroundImage == null)
        {
            Debug.LogError("BG Image is not assigned in the Inspector!");
            return;
        }

        _menuBg = backgroundImage.GetComponent<MenuBg>();

        if (_menuBg == null)
        {
            Debug.LogError("BG Image component is missing from the Settings Button!");
        }
    }
    
    public void ResetBackground()
    {
        _menuBg.SlideBackFromSettings();
    }
}
