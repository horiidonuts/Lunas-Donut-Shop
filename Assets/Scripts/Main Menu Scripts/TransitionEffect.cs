using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Scene changer is in this script!
public class TransitionEffect : MonoBehaviour
{
    [SerializeField] private Vector2 startWidth;
    [SerializeField] private Vector2 endWidth;
    [SerializeField] private Vector2 endSettingsWidth;
    
    [SerializeField] private float duration;
    [SerializeField] private float waitTime;
    [SerializeField] private int sceneIndex;

    
    //[SerializeField] private float loadSceneDelay;
    
    private RectTransform _rectTransform;
    
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = startWidth;
    }

    void Update()
    {
        _rectTransform.sizeDelta = startWidth;
    }

    public void TransitionIn()
    {
        StartCoroutine(SlideImage());
    }
    
    private IEnumerator SlideImage()
    {
        yield return new WaitForSeconds(waitTime);
        DOTween.To(() => startWidth, x => startWidth = x,
            endWidth, duration).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(duration);
        ChangeScene(sceneIndex);
    }

    private void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
