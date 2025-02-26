using DG.Tweening;
using UnityEngine;

public class CookBubblePop : MonoBehaviour
{
    RectTransform _rect;
    [SerializeField] private float _zoomDuration;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rect.transform.DOScale(new Vector3(0,0,0), 0f);
        PopBubble();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopBubble()
    {
        _rect.transform.DOScaleX(0.35f, _zoomDuration).SetEase(Ease.OutBack);
        _rect.transform.DOScaleY(0.35f, _zoomDuration).SetEase(Ease.OutBack);
    }
}
