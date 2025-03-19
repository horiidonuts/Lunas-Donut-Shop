using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TransitionEffect : MonoBehaviour
{
    [SerializeField] private Vector2 startWidth;
    [SerializeField] private Vector2 endWidth;
    [SerializeField] private Vector2 endSettingsWidth;

    [SerializeField] private float duration;
    [SerializeField] private float waitTime;
    private Vector2 _defaultWidth;
    private RectTransform _rectTransform;

    private RawImage _image;
    private Color _color;
    public static TransitionEffect Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        _defaultWidth = startWidth;
        _image = GetComponent<RawImage>();
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = startWidth;
        _color = _image.color;
    }

    void FixedUpdate()
    {
        _rectTransform.sizeDelta = startWidth;
        _image.color = _color;
    }

    public float GetDuration()
    {
        return duration;
    }

    public float GetWaitTime()
    {
        return waitTime;
    }

    public void TransitionIn()
    {
        StartCoroutine(SlideImage());
        Debug.Log("Translate in");
    }

    private IEnumerator SlideImage() // Koseden gelen overlayi kaydir ve kaymasi bitince kafe sahnesini yukle
    {
        yield return new WaitForSeconds(waitTime);
        DOTween.To(() => startWidth, x => startWidth = x,
            endWidth, duration).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(duration);
        ResetTransition();
    }

    private void ResetTransition() // Gecis efektini resetle
    {
        DOTween.To(() => startWidth, x => startWidth = x,
            _defaultWidth, 0f);
    }

    public void ChangeColorToBlack()
    {
        StartCoroutine(ColorToBlack());
    }

    public IEnumerator ColorToBlack()
    {
        Color black = new Color(0, 0, 0, 1);
        DOTween.To( () => _color, x => _color = x, black, duration).SetEase(Ease.Linear);
        yield return null;
    }
}