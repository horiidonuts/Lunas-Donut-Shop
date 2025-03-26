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
    
    [SerializeField] private Material lowerMaterial;
    [SerializeField] private Material upperMaterial;
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

        lowerMaterial = donutLower.GetComponent<Material>();
        upperMaterial = donutUpper.GetComponent<Material>();

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
            }
        }

        if (cookingUpper) // Cook upper side of the donut
        {
            if (!_tweenCalled)
            {
                _tweenCalled = true;
                DOTween.To(() => _upperMeter, x => _upperMeter = x,
                    maxCookingAmount, cookingTime).SetEase(Ease.Linear).SetId("CookingUpper");
            }
        }
        _currentlyCooking = true;
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
        cookingLower = !cookingLower;
        ResetCookingMeter();
        yield return new WaitForSeconds(resetDuration);
        cookingUpper = !cookingUpper;
        _tweenCalled = false;
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

    public float GetResetDuration()
    {
        return resetDuration;
    }
}