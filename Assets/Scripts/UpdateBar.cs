using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBar : MonoBehaviour
{
    //[SerializeField] private GameObject lunaDonut;
    
    private Image _image;
    private CookDonut _cookDonut;
    private float _cookingMeter;
    private bool _called = false;
    private float _redValue;
    private float _greenValue;
    void Start()
    {
        _cookDonut = GetComponentInParent<CookDonut>();
        _image = GetComponent<Image>();
        _image.fillAmount = 0;
        _redValue = 1;
    }
    
    void Update()
    {
        _cookingMeter = _cookDonut.GetCookingMeter();
        _image.fillAmount = _cookingMeter / 150;
        _image.color = new Color(_redValue, _greenValue, 0 , 1);
        if (_cookDonut.GetCookingStatus() && !_called)
        {
            ChangeBarColor();
        }
    }

    private void ChangeBarColor()
    {
         DOTween.To(() => _greenValue, x => _greenValue = x, 1, 
             _cookDonut.GetCookingTime()*0.5f).SetEase(Ease.Linear).OnComplete(
             () =>
             {
                 DOTween.To(() => _redValue, x => _redValue = x, 0, 
                     _cookDonut.GetCookingTime()*0.25f).OnComplete(
                     () =>
                     {
                         DOTween.To(() => _redValue, x => _redValue = x, 1,
                             _cookDonut.GetCookingTime()*0.125f).OnComplete(
                             () =>
                             {
                                 DOTween.To(() => _greenValue, x => _greenValue = x, 0, 
                                     _cookDonut.GetCookingTime()*0.125f);
                             }
                             );
                     }
                     );
             }
             );
         
        _called = true;
    }
}
