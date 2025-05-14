using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System.Numerics;

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

    [SerializeField] private Button _editButton;
    //private EditButton _editButtonScript;
    private Vector2 defaultScale;
    private Vector2 _pauseButtonPos;

    private RectTransform _pbRect;

    void Start()
    {
        // PausePanel'ı bul ve başlangıçta gizle
        pausePanel = GameObject.Find("PausePanel");
        defaultScale = pausePanel.transform.localScale;
        pausePanel.transform.localScale = new Vector3(0, 0, 0);
        pausePanel.SetActive(false);

        // Pause butonunu bul ve tıklama olayını dinle
        _pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        _pauseButton.onClick.AddListener(OpenPausePanel);
        _pauseButtonPos = _pauseButton.gameObject.transform.position;
        _pbRect = _pauseButton.GetComponent<RectTransform>();

        // Edit butonunu bul
        _editButton = GetComponent<Button>();
        //_editButtonScript = GameObject.Find("EditButton").GetComponent<EditButton>();
    }

    private void OpenPausePanel()
    {
        pausePanel.SetActive(true); // Önce panel aktif olmalı ki üzerinde işlem yapılsın.
        pausePanel.transform.DOScale(defaultScale, panelOpenDuration).SetEase(panelOpenEase); //Paneli açık konuma getir.

        _resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        _resumeButton.onClick.AddListener(ClosePausePanel); // Resume butonuna tıklandığında paneli kapat.

        _editButton.gameObject.transform.position = new Vector2(100, -90); // Edit butonunu pause butonunun pozisyonuna getir.

        _quitButton = GameObject.Find("HomeButton").GetComponent<Button>();
        _quitButton.onClick.AddListener(ReturnToMainMenu); // Ana menüye dön butonuna tıklandığında ana menüye dön.
    }

    public void ClosePausePanel()
    {
        // Pause panelini kapat
        pausePanel.transform.DOScale(new Vector2(0, 0), panelOpenDuration).SetEase(panelCloseEase).OnComplete(() =>
        {
            pausePanel.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            _pbRect.DOAnchorPos(new Vector2(100, 450), 0); // Pause butonunu eski pozisyonuna getir
        });  
    }

    private void ReturnToMainMenu()
    {
        StartCoroutine(ChangeScene(0)); // Ana menüye dön
    }

    private System.Collections.IEnumerator ChangeScene(int index)
    {
        // Ana menüye geçiş yap
        float duration = TransitionEffect.Instance.GetDuration();
        float waitTime = TransitionEffect.Instance.GetWaitTime();
        TransitionEffect.Instance.TransitionIn(0);
        yield return new WaitForSeconds(duration + waitTime);
    }

    public void EnablePanel()
    {
        pausePanel.SetActive(true);
    }

    public void DisablePanel()
    {
        pausePanel.SetActive(false);
    }

    public Vector2 GetPauseButtonPos()
    {
        return _pauseButtonPos;
    }
}