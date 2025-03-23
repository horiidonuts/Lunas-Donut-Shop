using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;

public class UpdateSlider : MonoBehaviour
{
    private Image _image;
    private Slider _slider;
    private float _cookingMeter;
    private float _cookingTime;
    private float _redValue;
    private float _greenValue;

    [SerializeField] private TextMeshProUGUI redText;
    [SerializeField] private TextMeshProUGUI greenText;
    

    void Start()
    {
        _image = GameObject.Find("CookingSliderFill").GetComponent<Image>();
        _slider = GetComponent<Slider>();
        _slider.value = 8;
        _redValue = 1;
        _greenValue = 0;
    }

    void Update()
    {
        _cookingTime = CookDonut.Instance.GetCookingTime();
        _cookingMeter = CookDonut.Instance.GetCookingMeter();

        if (CookDonut.Instance.GetCookingStatus())
        {
            if (_cookingMeter <= 50)
                _greenValue = _cookingMeter / 50f; // OK.
            else if (_cookingMeter is > 50 and <= 100)
                _redValue = 1 - (_cookingMeter - 50f) / 50f;
            else if (_cookingMeter is > 100 and <= 125)
                _redValue = (_cookingMeter - 100f) / 25f; // 1 = 125 - 100 / 25
            else if (_cookingMeter is > 125 and <= 150) _greenValue = 1 - (_cookingMeter - 125f) / 25f; // 0.5 = 1 - (125 - 100) / 50

            // Set the new adjusted colors every frame to their corresponding values at the end:
            
            _image.color = new Color(_redValue, _greenValue, 0, 1);
            redText.text =Math.Round(_redValue,2).ToString();
            greenText.text = Math.Round(_greenValue,2).ToString();
            if (_cookingMeter > 8)
            {
                _slider.value = _cookingMeter;
            }
        }
    }
}