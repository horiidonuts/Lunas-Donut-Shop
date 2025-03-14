using System;
using System.Collections;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TransitionEffect : MonoBehaviour
{
    [SerializeField] private Vector2 startWidth;
    [SerializeField] private Vector2 endWidth;
    [SerializeField] private Vector2 endSettingsWidth;

    [SerializeField] private float duration;
    [SerializeField] private float waitTime;
    //[SerializeField] private int sceneIndex;


    //[SerializeField] private float loadSceneDelay;
    private Vector2 _defaultWidth;
    private RectTransform _rectTransform;
    //private SceneManager _sceneManager;

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
        //_sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = startWidth;
    }

    void FixedUpdate()
    {
        _rectTransform.sizeDelta = startWidth;
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
}