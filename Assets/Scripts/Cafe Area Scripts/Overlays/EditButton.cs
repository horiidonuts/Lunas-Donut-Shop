using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditButton : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private GameObject _buttonObj;
    private Button _button;
    private Vector2 _originalSize;
    private GameObject _buttonText;
    [SerializeField] private Vector2 targetSize;
    [SerializeField] private Ease easeIn;
    [SerializeField] private Ease easeOut;
    [SerializeField] private bool editMode;
    [SerializeField] private Sprite exitSprite;
    [SerializeField] private float moveAmount;

    [SerializeField] private GameObject _pauseButton;
    private PauseButton _pauseButtonScript;

    private RectTransform _btnRect;
    private Vector2 _pauseButtonPos;

    private void Awake()
    {
        _button = _buttonObj.GetComponent<Button>();
        _btnRect = _buttonObj.GetComponent<RectTransform>();
    }

    void Start()
    {
         //Get components of _pauseButton
        _pauseButtonScript = _pauseButton.GetComponent<PauseButton>();

        if (!_pauseButtonScript)
        {
            Debug.LogError("PauseButtonScript not assigned in EditButton script.");
            return;
        }

        _buttonText = _button.GetComponentInChildren<TextMeshProUGUI>().gameObject;
        _button.onClick.AddListener(Click);
        _originalSize = _button.transform.localScale;
        Debug.Log("Edit button will move to:" + _pauseButtonPos);
    }

    private void Click()
    {
        ButtonAnimation();

        if (editMode) // Edit modda ise tiklayinca bunlari yap
        {
            _buttonText.SetActive(true);
            _button.image.overrideSprite = null;
            _pauseButton.SetActive(true); // Pause butonunu göster
            StartCoroutine(SetPanelAsParent());
            //_pauseButton.gameObject.SetActive(false); 

            /*EDIT TUSUNA BASINCA PAUSE TUSUNUN KAYBOLMASI GEREK*/
        }

        if (!editMode) // Edit modda değilse tiklayinca bunlari yap
        {
            _btnRect.anchorMin = new Vector2(0, 1);
            _btnRect.anchorMax = new Vector2(0, 1);

            _pauseButtonPos = _pauseButtonScript.GetPauseButtonPos();
            _pauseButtonScript.ClosePausePanel();
            _btnRect.DOAnchorPos(_pauseButtonPos, 0);
            _button.image.overrideSprite = exitSprite;
            _buttonText.SetActive(false);
            _pauseButton.SetActive(false); // Pause butonunu gizle
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
        _pauseButtonScript.EnablePanel();
        _button.gameObject.SetActive(true);
        _button.transform.SetParent(GameObject.Find("PausePanel").transform, true);
        _btnRect.anchorMin = new Vector2(1f, 0.5f);
        _btnRect.anchorMax = new Vector2(1f, 0.5f);
        _btnRect.DOAnchorPos(new Vector2(-100, 0), 0);
        yield return new WaitForSeconds(0.05f);
        _pauseButtonScript.DisablePanel();
    }
}
