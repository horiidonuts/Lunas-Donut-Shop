using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;

public class PauseButton : MonoBehaviour
{
    /* [SerializeField] */
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private float panelOpenDuration;
    [SerializeField] private Ease panelOpenEase;
    [SerializeField] private Ease panelCloseEase;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private GameObject _editButtonObj;
    private Button _editButton;
    private EditButton _editButtonScript;
    private Vector2 defaultScale;
    private Vector2 _pauseButtonPos;

    private RectTransform _pbRect;

    void Start()
    {
        _editButton = _editButtonObj.GetComponent<Button>();
        _pbRect = _pauseButton.GetComponent<RectTransform>();
        _pauseButton = _pauseButton.GetComponent<Button>();
        _editButtonScript = _editButtonObj.GetComponent<EditButton>();

        // PausePanel'ı bul ve başlangıçta gizle

        defaultScale = pausePanel.transform.localScale;
        pausePanel.transform.localScale = new Vector3(0, 0, 0);
        pausePanel.SetActive(false);

        // Pause butonunu bul ve tıklama olayını dinle
        _pauseButton.onClick.AddListener(OpenPausePanel);

        _pauseButtonPos = _pbRect.anchoredPosition;


        Debug.Log(_pauseButtonPos);
    }

    private void OpenPausePanel()
    {
        pausePanel.SetActive(true); // Önce panel aktif olmalı ki üzerinde işlem yapılsın.
        pausePanel.transform.DOScale(defaultScale, panelOpenDuration).SetEase(panelOpenEase); //Paneli açık konuma getir.

        _resumeButton = _resumeButton.GetComponent<Button>();
        _resumeButton.onClick.AddListener(ClosePausePanel); // Resume butonuna tıklandığında paneli kapat.

        //_editButton.gameObject.transform.position = new Vector2(100, -90); // Edit butonunu kendi yerine getir.

        // _quitButton = _quitButton.GetComponent<Button>();
        // _quitButton.onClick.AddListener(ReturnToMainMenu); // Ana menüye dön butonuna tıklandığında ana menüye dön.

        _pauseButton.gameObject.SetActive(false); // Pause butonunu gizle
    }

    public void ClosePausePanel()
    {
        // Pause panelini kapat
        pausePanel.transform.DOScale(new Vector2(0, 0), panelOpenDuration).SetEase(panelCloseEase).OnComplete(() =>
        {
            pausePanel.SetActive(false);

            if (_editButtonScript.IsInEditMode())
            {
                _pauseButton.gameObject.SetActive(false); // Edit modunda pause butonunu gizle
            }
            if (!_editButtonScript.IsInEditMode())
            {
                _pauseButton.gameObject.SetActive(true); // Edit modunda değilse pause butonunu göster
            }
            _pbRect.DOAnchorPos(new Vector2(100, -90), 0); // Pause butonunu eski pozisyonuna getir
        });
    }

    private void ReturnToMainMenu()
    {
        
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