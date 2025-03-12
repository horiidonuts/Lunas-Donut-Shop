using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

public class MenuBg : MonoBehaviour
{
    private float _width;
    [SerializeField] private float endWidth;
    [SerializeField] private float duration;
    [SerializeField] Ease easing;
    
    private RawImage _image;
    void Start()
    {
        _image = GetComponent<RawImage>();
        _width = _image.uvRect.width;
    }

    void Update()
    {
        _image.uvRect = new Rect(0f, 0f, _width, 1f);
    }

    public void SlideImage()
    {
        DOTween.To(() => _width, x => _width = x,endWidth , duration).SetEase(easing);
    }
}
