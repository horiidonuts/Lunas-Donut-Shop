using System;
using DG.Tweening;
using UnityEngine;

public class TransitionOut : MonoBehaviour
{
    [SerializeField] private Vector2 startSize;
    [SerializeField] private Vector2 endSize;
    [SerializeField] private float duration;

    private RectTransform _rectTransform;
    private Vector2 _defaultSize;
    public static TransitionOut Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
         _defaultSize = startSize;
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = startSize;
        TranslateOut();
    }

    void Update()
    {
        _rectTransform.sizeDelta = startSize;
    }

    public void TranslateOut()
    {
        DOTween.To(() => startSize, x => startSize = x,
            endSize, duration).SetEase(Ease.OutQuad);
        Debug.Log("Translated Out");
    }

    public void ResetTransition()
    {
        DOTween.To(() => startSize, x => startSize = x,
            _defaultSize, 0f);
        Debug.Log("Reset transition out size");
    }

    public float GetDuration()
    {
        return duration;
    }
}