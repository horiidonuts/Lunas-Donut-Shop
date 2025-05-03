using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CookDonut : MonoBehaviour
{
    [SerializeField] private GameObject donutLower;
    [SerializeField] private GameObject donutUpper;
    [SerializeField] private float cookingMeter;

    [SerializeField] private float maxCookingAmount;

    [SerializeField] private float cookingTime;
    [SerializeField] private float resetDuration;

    private float _elapsedUnResetTime;
    
    
    // [SerializeField] private Material lowerMaterial;
    // [SerializeField] private Material upperMaterial;
    [SerializeField] private Material material;
    [SerializeField] private Color maxColor;
    [SerializeField] private Ease easing;
    
    
    
    [FormerlySerializedAs("_cookingUpper")] [SerializeField] private bool cookingUpper;
    [FormerlySerializedAs("_cookingLower")] [SerializeField] private bool cookingLower;
    private float _lowerMeter;
    private float _upperMeter;
    private bool _currentlyCooking = false;

    private bool _tweenCalled;

    public static CookDonut Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        cookingMeter = 0;

        // lowerMaterial = donutLower.GetComponent<Material>();
        // upperMaterial = donutUpper.GetComponent<Material>();
        
        material = donutLower.GetComponent<MeshRenderer>().material;
        material.color = new Color(1f, 0.9f, 0.8f, 1f);
        
        cookingMeter = Mathf.Clamp(cookingMeter, 0, maxCookingAmount);
        _lowerMeter = Mathf.Clamp(_lowerMeter, 0, maxCookingAmount);
        _upperMeter = Mathf.Clamp(_upperMeter, 0, maxCookingAmount);

        cookingLower = true;
        _tweenCalled = false;
    }

    private void FixedUpdate()
    {
        IncreaseCookingMeter();

        if (cookingUpper)
        {
            cookingMeter = _upperMeter;
        }

        if (cookingLower)
        {
            cookingMeter = _lowerMeter;
        }
    }

    private void IncreaseCookingMeter()
    {        
        if (cookingLower) // Cook lower side of the donut
        {
            if (!_tweenCalled)
            {
                _tweenCalled = true;
                DOTween.To(() => _lowerMeter, x => _lowerMeter = x,
                    maxCookingAmount, cookingTime).SetEase(Ease.Linear).SetId("CookingLower");
                // color (1, 0.9, 0.8) -> color (0.88, 0.6, 0.325)
                ChangeMaterialColor();
            }
        }

        if (cookingUpper) // Cook upper side of the donut
        {
            if (!_tweenCalled)
            {
                _tweenCalled = true;
                DOTween.To(() => _upperMeter, x => _upperMeter = x,
                    maxCookingAmount, cookingTime).SetEase(Ease.Linear).SetId("CookingUpper");
                ChangeMaterialColor();
            }
        }
        _currentlyCooking = true;
    }

    private void ChangeMaterialColor()
    {
        DOTween.To(() => material.color, x => material.color = x,
            maxColor, cookingTime).SetEase(easing).SetId("Cooking");
    }

    private void ResetCookingMeter()
    {
        DOTween.To(() => cookingMeter, x => cookingMeter = x,
                0f, resetDuration).SetEase(Ease.OutQuint);
    }

    public void ChangeSides()
    {
        StartCoroutine(ChangeSidesDelay());
    }

    private IEnumerator ChangeSidesDelay()
    {
        DOTween.Kill("CookingLower");
        DOTween.Kill("Cooking");
        cookingLower = !cookingLower;
        ResetCookingMeter();
        yield return new WaitForSeconds(resetDuration);
        ChangeMaterialCooking();
        cookingUpper = !cookingUpper;
        _tweenCalled = false;
    }

    private void ChangeMaterialCooking()
    {
        material = donutUpper.GetComponent<MeshRenderer>().material;
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

    public void StopCooking()
    {
        DOTween.Kill("Cooking");
    }
}