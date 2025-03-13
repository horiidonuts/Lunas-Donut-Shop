using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class MenuBg : MonoBehaviour
{
    private float _width; // Arkaplanın ilk değiştirilecek olan genişliği
    [SerializeField] private float endWidth; // Arkaplanın ayarlanacağı değer
    [SerializeField] private float settingEndWidth; // Üsttekinin aynısı ama ayarlar menüsü için
    [SerializeField] private float duration; // Kayma süresi
    [SerializeField] Ease easing; // Kayma sönümlemesi
    [SerializeField] private Ease settingsEasing;
    [SerializeField] private float defaultWidth; // Ana menudeki default genislik
    
    
    private RawImage _image;
    void Start()
    {
        _image = GetComponent<RawImage>();
        //_width = _image.uvRect.width;
        ReturnedToMenu();
    }

    void Update()
    {
        _image.uvRect = new Rect(0f, 0f, _width, 1f); // Değiştirdiğimiz genişliği her frame'de UV Rect'e eşitliyoruz
    }

    public void SlideImage() // Oyunu başlatırkenki kaydırma efekti
    {
        DOTween.To(() => _width, x => _width = x,endWidth , duration).SetEase(easing);
    }

    public void SlideImageForSettings() // Ayarlar menüsü için farklı kaydırma efekti
    {
        DOTween.To(() => _width, x => _width = x, settingEndWidth, duration).SetEase(settingsEasing);
    }

    public void SlideBackFromSettings()
    {
        DOTween.To(() => _width, x => _width = x, defaultWidth, duration).SetEase(settingsEasing);
    }

    public void ReturnedToMenu()
    {
        _width = 1.5f;
        DOTween.To(() => _width, x => _width = x, defaultWidth, duration).SetEase(settingsEasing);
    }
}
