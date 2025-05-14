using System.Collections;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class EditButton : MonoBehaviour
{
    [SerializeField] private float duration;

    private Button _button;
    private Vector2 _originalSize;
    private GameObject _buttonText;
    [SerializeField] private Vector2 targetSize;
    [SerializeField] private Ease easeIn;
    [SerializeField] private Ease easeOut;
    [SerializeField] private bool editMode;
    [SerializeField] private Sprite exitSprite;
    [SerializeField] private float moveAmount;

    private PauseButton _pauseButton;

    private RectTransform _btnRect;
    private Vector2 _pauseButtonPos;

    private void Awake()
    {
        _pauseButton = GameObject.Find("PauseButton").GetComponent<PauseButton>();
    }

    void Start()
    {
        _pauseButtonPos = _pauseButton.GetPauseButtonPos();
        _buttonText = GameObject.Find("EditButtonText");
        _button = GetComponent<Button>();
        _btnRect = GetComponent<RectTransform>();
        _button.onClick.AddListener(Click);
        _originalSize = _button.transform.localScale;
    }

    private void Click()
    {
        Debug.Log(_pauseButtonPos);
        ButtonAnimation();

        if (editMode) // Edit modda ise tiklayinca bunlari yap
        {
            _buttonText.SetActive(true);
            _button.image.overrideSprite = null;
            StartCoroutine(SetPanelAsParent());
            //_pauseButton.gameObject.SetActive(false); 

            /*EDIT TUSUNA BASINCA PAUSE TUSUNUN KAYBOLMASI GEREK*/

            // En son butonu edit moddan çıkarttığımızda edit butonunu UI'nin altına alıyoruz.
        }

        if (!editMode) // Edit modda değilse tiklayinca bunlari yap
        {
            _pauseButton.ClosePausePanel();
            _btnRect.DOMove(_pauseButtonPos, 0);
            _button.image.overrideSprite = exitSprite;
            _buttonText.SetActive(false);
            _button.transform.SetParent(GameObject.Find("UI").transform, false);
        }

        editMode = !editMode; // Edit modunu değiştir
        Debug.Log("Edit Mode: " + editMode);
    }

    private void ButtonAnimation() // Buton animasyonunu kontrol eden method
    {
        _button.interactable = false;
        _button.transform.DOScale(targetSize, duration / 2).SetEase(easeIn).OnComplete(() =>
        {
            _button.transform.DOScale(_originalSize, duration / 2).SetEase(easeOut).OnComplete(() =>
                {
                    _button.interactable = true;
                });
        });
    }

    public bool IsInEditMode()
    {
        return editMode;
    }

    private IEnumerator SetPanelAsParent()
    {
        _pauseButton.EnablePanel();
        _button.gameObject.SetActive(true);
        _btnRect.DOAnchorPos(new Vector2(100, -90), 0).SetEase(Ease.OutQuint);
        _button.transform.SetParent(GameObject.Find("PausePanel").transform, true);
        yield return new WaitForSeconds(0.05f);
        _pauseButton.DisablePanel();
    }
}
