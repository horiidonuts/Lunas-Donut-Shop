using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class EditButton : MonoBehaviour
{
    [SerializeField] private float duration;
    
    
    private RectTransform _rectTransform;
    private Button _button;
    private Vector2 _originalSize;
    private GameObject _buttonText;
    [SerializeField] private Vector2 targetSize;
    [SerializeField] private Ease easeIn;
    [SerializeField] private Ease easeOut;
    [SerializeField] private bool editMode;
    [SerializeField] private Sprite exitSprite;
    
    
    
    void Start()
    {
        _buttonText = GameObject.Find("EditButtonText");
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
        // Debug.Log(_button.transform.localScale);
        _originalSize = _button.transform.localScale;
    }

    private void Click()
    {
        ButtonAnimation();
        editMode = !editMode;
        
        if (editMode)
        {
            _button.image.overrideSprite = exitSprite;
            _buttonText.SetActive(false);
        }
        else
        {
            _button.image.overrideSprite = null;
            _buttonText.SetActive(true);
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
