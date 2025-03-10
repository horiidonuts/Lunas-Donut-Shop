using System;
using DG.Tweening;
using UnityEngine;

public class TransitionOut : MonoBehaviour
{
    [SerializeField] private Vector2 startSize;
    [SerializeField] private Vector2 endSize;
    [SerializeField] private float duration;
    
    private RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = startSize;
        TranslateOut();
    }

    void Update()
    {
        _rectTransform.sizeDelta = startSize;
    }

    private void TranslateOut()
    {
        DOTween.To(() => startSize, x => startSize = x, endSize, duration).SetEase(Ease.OutQuad);
    }
}
