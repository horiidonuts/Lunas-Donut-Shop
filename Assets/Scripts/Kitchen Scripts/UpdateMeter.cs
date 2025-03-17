using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class UpdateMeter : MonoBehaviour
{
    [SerializeField] private int magnitude = 1;
    [SerializeField] private int randomness;
    [SerializeField] private int vibrato;
    [SerializeField] private int maxMagnitude;
    
    private TextMeshProUGUI _textMeshPro;
    //private CookDonut _cookDonut;
    private RectTransform _rect;
    private bool _called = false;
    private Vector3 _defaultPosition;
    
    void Start()
    {
        //_cookDonut = GetComponentInParent<CookDonut>();
        // if (_cookDonut == null)
        // {
        //     Debug.LogError("CookDonut component not found!");
        // }

        _textMeshPro = GetComponent<TextMeshProUGUI>();
        if (_textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found!");
        }

        _rect = GetComponent<RectTransform>();
        if (_rect == null)
        {
            Debug.LogError("RectTransform component not found");
        }

        _defaultPosition = _rect.localPosition;
    }
    
    private void FixedUpdate()
    {
        _textMeshPro.text = ("Cooking... ["  + (int) CookDonut.instance.GetCookingMeter()+ "]");
        
        if (!_called && CookDonut.instance.GetCookingMeter() >= 100 )
        {
            TweenMagnitude();
        }
        
        if (CookDonut.instance.GetCookingMeter() >= 110 && magnitude % 2 == 0 && magnitude != 30) // Metre > ise, magnitude her 2 arttiginda ve magnitude < 30 ise
        {   
            ResetPosition(); // Pozisyonu resetle/ortala
            ShakeOnOvercook(); // Texti sallandir
        }
    }

    private void ShakeOnOvercook()
    {
        if (float.IsNaN(_rect.localPosition.x) || float.IsNaN(_rect.localPosition.y))
        {
            Debug.LogError("localPosition has NaN values! Resetting position.");
            _rect.localPosition = _defaultPosition;  // Reset to a safe value
        }

        _rect.DOShakePosition(1f, new Vector3(magnitude, magnitude, 0f),
            vibrato, randomness, true, false).SetId("Shake").SetLoops(-1, LoopType.Restart); // I'm tweakin rn
        // _called = true;
    }

    private void TweenMagnitude()
    {
        DOTween.To(() => magnitude, x => magnitude = x, maxMagnitude, 
            CookDonut.instance.GetCookingTime()*0.25f).SetEase(Ease.Linear);
        _called = true;
    }

    private void ResetPosition()
    {
        DOTween.Kill("Shake");
        _rect.DOLocalMove(_defaultPosition, 0.1f);
    }
}