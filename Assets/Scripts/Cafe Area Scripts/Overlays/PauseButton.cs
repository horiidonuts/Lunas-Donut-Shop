using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.IMGUI.Controls;
using DG.Tweening;
using Unity.Burst.Intrinsics;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Unity.VisualScripting;
using Vector2 = UnityEngine.Vector2;

public class PauseButton : MonoBehaviour
{
    /* [SerializeField] */
    private GameObject pausePanel;
    [SerializeField] private float panelOpenDuration;
    [SerializeField] private Ease panelOpenEase;
    [SerializeField] private Ease panelCloseEase;
    private Button _pauseButton;
    private Button _resumeButton;
    private Button _quitButton;

    private GameObject _editButton;
    //private EditButton _editButtonScript;
    private Vector3 defaultScale;

    private GameObject _editBtnTransform;
    private Vector3 _editBtnDefaultTransform;

    void Start()
    {
        pausePanel = GameObject.Find("PausePanel");
        defaultScale = pausePanel.transform.localScale;
        pausePanel.transform.localScale = new Vector3(0, 0, 0);
        pausePanel.SetActive(false);

        _pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        _pauseButton.onClick.AddListener(OpenPausePanel);

        _editButton = GameObject.Find("EditButton");
        //_editButtonScript = GameObject.Find("EditButton").GetComponent<EditButton>();
        _editBtnTransform = GameObject.Find("EditButtonMove");
        _editBtnDefaultTransform = _editButton.transform.position;
    }

    private void OpenPausePanel()
    {
        pausePanel.SetActive(true); // Önce panel aktif olmalı ki üzerinde işlem yapılsın.
        pausePanel.transform.DOScale(defaultScale, panelOpenDuration).SetEase(panelOpenEase);
        _resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        _resumeButton.onClick.AddListener(ClosePausePanel);
        _quitButton = GameObject.Find("HomeButton").GetComponent<Button>();
        _quitButton.onClick.AddListener(ReturnToMainMenu);
        
    }

    public void ClosePausePanel()
    {
        pausePanel.transform.DOScale(new Vector3(0, 0, 0), panelOpenDuration).SetEase(panelCloseEase).OnComplete(() =>
        {
            pausePanel.SetActive(false);
        });
    }

    private void ReturnToMainMenu()
    {
        StartCoroutine(ChangeScene(0));
    }

    private System.Collections.IEnumerator ChangeScene(int index)
    {
        float duration = TransitionEffect.Instance.GetDuration();
        float waitTime = TransitionEffect.Instance.GetWaitTime();
        TransitionEffect.Instance.TransitionIn(0);
        yield return new WaitForSeconds(duration + waitTime);
    }

    public Vector3 EditButtonDefaultPos()
    {
        return _editBtnDefaultTransform;
    }
}