using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UpdateMeter : MonoBehaviour
{
    TextMeshProUGUI _textMeshPro;
    [SerializeField] private GameObject lunaDonut;
    private CookDonut _cookDonut;

    void Start()
    {
        _cookDonut = lunaDonut.GetComponent<CookDonut>();
        if (_cookDonut == null)
        {
            Debug.LogError("CookDonut component not found!");
        }

        _textMeshPro = GetComponent<TextMeshProUGUI>();
        if (_textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found!");
        }
    }
    
    private void FixedUpdate()
    {
        _textMeshPro.text = ("Cooking: "  + (int) _cookDonut.GetCookingMeter());
        
    }
}