using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpdateSlider : MonoBehaviour
{
    private Image _image;
    private Slider _slider;
    private float _cookingMeter;
    private float _cookingTime;
    private float _redValue;
    private float _greenValue;
    private bool _called = false;

    void Start()
    {
        _image = GameObject.Find("CookingSliderFill").GetComponent<Image>();
        _slider = GetComponent<Slider>();
        _slider.value = 8;
        _redValue = 1;
    }

    void Update()
    {
        _cookingTime = CookDonut.Instance.GetCookingTime();
        _cookingMeter = CookDonut.Instance.GetCookingMeter();
        _image.color = new Color(_redValue, _greenValue, 0, 1);

        if (_cookingMeter > 8 && CookDonut.Instance.GetCookingStatus())
        {
            if (!_called)
            {
                ChangeBarColor();
            }
            _slider.value = _cookingMeter;
        }
    }

    private void ChangeBarColor()
    {
        DOTween.To(() => _greenValue, x => _greenValue = x, 1,
            _cookingTime* 0.5f).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                DOTween.To(() => _redValue, x => _redValue = x, 0,
                    _cookingTime * 0.25f).OnComplete(
                    () =>
                    {
                        DOTween.To(() => _redValue, x => _redValue = x, 1,
                            _cookingTime * 0.125f).OnComplete(
                            () =>
                            {
                                DOTween.To(() => _greenValue, x => _greenValue = x, 0,
                                    _cookingTime * 0.125f);
                            }
                        );
                    }
                );
            }
        );
        _called = true;
    }
}