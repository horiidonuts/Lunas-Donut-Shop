using System;
using TMPro;
using UnityEngine;

public class CookDonut : MonoBehaviour
{
    [SerializeField] private GameObject donutLower;
    [SerializeField] private GameObject donutUpper;
    [SerializeField] private float cookingMeter;
    [SerializeField] private float cookingFactor;
    [SerializeField] private int maxCookingAmount;
    [SerializeField] private float cookingIncrement;
    private float _elapsedUnResetTime;
    private float _cookingMeterSlider;
    
    
    private Material _lowerMaterial;
    private Material _upperMaterial;
    private bool _cookingUpper = false;
    private bool _cookingLower = false;
    private float _elapsedTime = 0f;
    //private int _elapsedSeconds = 0;
    
    
    
    void Start()
    {
        cookingMeter = 0;
        _cookingMeterSlider = cookingMeter;
        _lowerMaterial = donutLower.GetComponent<Material>();
        _upperMaterial = donutUpper.GetComponent<Material>();
        
        cookingMeter = Mathf.Clamp(cookingMeter, 0, maxCookingAmount);
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        
        
        // if (_elapsedTime >= 1)
        // {
        //     _elapsedSeconds++;
        //     IncreaseCookingMeter();
        //     // Debug.Log(_seconds);
        //     _elapsedTime -= 1;
        // }
        
        // Debug.Log(cookingMeter);
        
        
        
        if (_cookingUpper)
        {
            // Cook upper side of the donut
        }
        
        if (_cookingLower)
        {
            // Cook lower side of the donut
        }
    }

    private void FixedUpdate()
    {
        _elapsedUnResetTime += Time.fixedDeltaTime;
        IncreaseCookingMeter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CookingPan"))
        {
            _cookingLower = true;
        }
    }

    private void IncreaseCookingMeter()
    {
        //cookingMeter += Mathf.RoundToInt(cookingFactor * _elapsedSeconds);
        cookingMeter += cookingIncrement;
    }

    public void ChangeSides()
    {
        _cookingUpper = !_cookingUpper;
        _cookingLower = !_cookingLower;
    }

    public float GetCookingMeter()
    {
        return cookingMeter;
    }
}
