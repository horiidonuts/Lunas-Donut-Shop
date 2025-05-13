using DG.Tweening;
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

    private PauseButton _pauseButton;

    void Start()
    {
        _pauseButton = GameObject.Find("PauseButton").GetComponent<PauseButton>();
        _buttonText = GameObject.Find("EditButtonText");
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
        _originalSize = _button.transform.localScale;
    }

    private void Click()
    {
        // if (editMode)
        // {
        //     _button.transform.position = _pauseButton.EditButtonDefaultPos();
        // }
        ButtonAnimation();
        _pauseButton.ClosePausePanel();
        editMode = !editMode;

        if (editMode)
        {
            _button.image.overrideSprite = exitSprite;
            _buttonText.SetActive(false);
            _button.transform.SetParent(GameObject.Find("UI").transform, true);
            _button.transform.DOMove(new Vector3(_button.transform.position.x - 100, _button.transform.position.y-100, 0), 0.5f);
        }
        if (!editMode)
        {
            _buttonText.SetActive(true);
            _button.image.overrideSprite = null;
            _button.transform.SetParent(GameObject.Find("PausePanel").transform, true);
        }
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
}
