using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Image image;
    private float _pixelMultiplier = 125f;
    void Start()
    {
        image = GetComponent<Image>();
        
    }
    
    void Update()
    {
        image.pixelsPerUnitMultiplier = _pixelMultiplier;

        Image.Type oldType = image.type;
        image.type = Image.Type.Simple;
        image.type = oldType;
    }

    public void Play()
    {
        Debug.Log("Clicked");
        DOTween.To(() => _pixelMultiplier, x => _pixelMultiplier = x, 3.25f,
            1f).SetEase(Ease.OutExpo);
    }
}
