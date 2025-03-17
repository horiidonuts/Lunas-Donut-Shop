using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CookDonut : MonoBehaviour
{
    [SerializeField] private GameObject donutLower;
    [SerializeField] private GameObject donutUpper;
    [SerializeField] private float cookingMeter;

    [SerializeField] private int maxCookingAmount;

    // [SerializeField] private float cookingIncrement;
    [SerializeField] private float cookingTime;
    [SerializeField] private float resetDuration;
    

    private float _elapsedUnResetTime;
    private float _cookingMeterSlider;


    private Material _lowerMaterial;
    private Material _upperMaterial;
    private bool _cookingUpper = false;
    private bool _cookingLower = false;
    private bool _currentlyCooking = false;

    public static CookDonut instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


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
        //Debug.Log(cookingMeter);
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
        if (!_currentlyCooking)
        {
            IncreaseCookingMeter();
        }
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
        DOTween.To(() => cookingMeter, x => cookingMeter = x,
            150, cookingTime).SetEase(Ease.Linear).SetId("CookingMeter");
        _currentlyCooking = true;
    }
    
    public void ResetCookingMeter()
    {
        DOTween.Kill("CookingMeter");
        DOTween.To(() => cookingMeter, x => cookingMeter = x,
            0f, resetDuration).SetEase(Ease.OutQuint).OnComplete(IncreaseCookingMeter);
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

    public bool GetCookingStatus()
    {
        return _currentlyCooking;
    }

    public float GetCookingTime()
    {
        return cookingTime;
    }
}